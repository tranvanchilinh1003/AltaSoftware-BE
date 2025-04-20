using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AccountController : ControllerBase
{
    private readonly IPasswordResetService _passwordResetService;
    private readonly IEmailService _emailService;
    private readonly IUserService _userService;

    public AccountController(IPasswordResetService passwordResetService, IEmailService emailService, IUserService userService)
    {
        _passwordResetService = passwordResetService;
        _emailService = emailService;
        _userService = userService;
    }

    [HttpPost("request-password-reset")]
    [AllowAnonymous]
    public async Task<IActionResult> RequestPasswordReset([FromBody] PasswordResetRequest request)
    {
        var user = await _passwordResetService.GetUserByEmailAsync(request.Email);
        if (user == null)
        {
            return new ObjectResult(ApiResponse<string>.NotFound("Email không đúng"))
            {
                StatusCode = 404
            };
        }

        var otp = new Random().Next(100000, 999999).ToString();
        await _passwordResetService.SaveOtpAsync(user.Id, otp);

        var emailSent = await _emailService.SendEmailAsync(user.Email, "Mã OTP", otp);
        if (!emailSent)
        {
            return new ObjectResult(ApiResponse<string>.Fail("Gửi email thất bại"))
            {
                StatusCode = 500
            };
        }

        return Ok(ApiResponse<string>.Success("OTP đã được gửi đến email của bạn"));
    }

    [HttpPost("verify-otp")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyOtp([FromBody] PasswordResetOtpVerificationRequest request)
    {
        var user = await _passwordResetService.GetUserByEmailAsync(request.Email);
        if (user == null)
        {
            return new ObjectResult(ApiResponse<string>.NotFound("Email không đúng"))
            {
                StatusCode = 404
            };
        }

        var isValidOtp = await _passwordResetService.VerifyOtpAsync(user.Id, request.Otp);
        if (!isValidOtp)
        {
            return new ObjectResult(ApiResponse<string>.BadRequest("OTP không hợp lệ"))
            {
                StatusCode = 400
            };
        }

        var temporaryPassword = GenerateRandomPassword();
        var updatePasswordResponse = _userService.UpdateUserPassword(user.Id, temporaryPassword);
        if (updatePasswordResponse.Code == 1)
        {
            return new ObjectResult(updatePasswordResponse)
            {
                StatusCode = 400
            };
        }

        await _passwordResetService.SaveTemporaryPasswordAsync(user.Id, temporaryPassword);

        var emailSent = await _emailService.SendEmailAsync(user.Email, "Mật khẩu tạm thời của bạn", temporaryPassword);
        if (!emailSent)
        {
            return new ObjectResult(ApiResponse<string>.Fail("Gửi email thất bại"))
            {
                StatusCode = 500
            };
        }

        return Ok(ApiResponse<string>.Success("Mật khẩu tạm thời đã được gửi đến email của bạn"));
    }

    [HttpPost("update-password")]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordByEmailRequest request)
    {
        var user = await _passwordResetService.VerifyTemporaryPasswordAsync(request.Email, request.TemporaryPassword);
        if (user == null)
        {
            return new ObjectResult(ApiResponse<string>.BadRequest("Mật khẩu tạm thời không hợp lệ hoặc đã hết hạn"))
            {
                StatusCode = 400
            };
        }

        var updatePasswordResponse = _userService.UpdateUserPassword(user.Id, request.NewPassword);
        if (updatePasswordResponse.Code == 1)
        {
            return new ObjectResult(updatePasswordResponse)
            {
                StatusCode = 400
            };
        }

        await _passwordResetService.InvalidateTemporaryPasswordAsync(user.Id);

        return Ok(ApiResponse<string>.Success("Mật khẩu đã được cập nhật thành công"));
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userId = User.FindFirst("id")?.Value;
        if (userId == null)
        {
            return Unauthorized(ApiResponse<string>.Unauthorized("Người dùng không xác thực"));
        }

        var changePasswordResponse = _userService.ChangePassword(int.Parse(userId), request.CurrentPassword, request.NewPassword);
        if (changePasswordResponse.Code != 0)
        {
            return BadRequest(changePasswordResponse);
        }

        return Ok(ApiResponse<string>.Success("Mật khẩu đã được cập nhật thành công"));
    }

    private string GenerateRandomPassword()
    {
        const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        StringBuilder sb = new StringBuilder();
        using (var rng = RandomNumberGenerator.Create())
        {
            byte[] uintBuffer = new byte[sizeof(uint)];

            while (sb.Length < 8)
            {
                rng.GetBytes(uintBuffer);
                uint num = BitConverter.ToUInt32(uintBuffer, 0);
                sb.Append(validChars[(int)(num % (uint)validChars.Length)]);
            }
        }

        return sb.ToString();
    }

}
