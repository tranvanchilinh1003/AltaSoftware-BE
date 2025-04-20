using ISC_ELIB_SERVER.Models;

public interface IPasswordResetService
{
    Task<User> GetUserByEmailAsync(string email);
    Task SaveOtpAsync(int userId, string otp);
    Task<bool> VerifyOtpAsync(int userId, string otp);  
    Task SaveTemporaryPasswordAsync(int userId, string temporaryPassword);
    Task<User?> VerifyTemporaryPasswordAsync(string email, string temporaryPassword);
    Task InvalidateTemporaryPasswordAsync(int userId);
}
