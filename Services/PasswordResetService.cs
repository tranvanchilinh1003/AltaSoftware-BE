using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class PasswordResetService : IPasswordResetService
{
    private readonly isc_dbContext _context;
    private readonly ITemporaryPasswordService _tempPasswordService;

    public PasswordResetService(isc_dbContext context, ITemporaryPasswordService tempPasswordService)
    {
        _context = context;
        _tempPasswordService = tempPasswordService;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task SaveOtpAsync(int userId, string otp)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            user.SetOtp(otp);
            user.SetOtpExpiration(DateTime.UtcNow.AddMinutes(3)); 
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> VerifyOtpAsync(int userId, string otp)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            return user.GetOtp() == otp && user.GetOtpExpiration() > DateTime.UtcNow;
        }
        return false;
    }

    public async Task SaveTemporaryPasswordAsync(int userId, string temporaryPassword)
    {
        await Task.Run(() => {
        });

        var record = new TemporaryPasswordRecord
        {
            UserId = userId,
            Password = temporaryPassword, 
            CreatedAt = DateTime.UtcNow
        };
        await _tempPasswordService.SaveTemporaryPasswordAsync(record);
    }

    public async Task<User?> VerifyTemporaryPasswordAsync(string email, string temporaryPassword)
    {
        var user = await GetUserByEmailAsync(email);
        if (user == null)
            return null;

        var record = await _tempPasswordService.GetByUserIdAsync(user.Id);
        if (record == null)
            return null;

        if (record.Password == temporaryPassword && (DateTime.UtcNow - record.CreatedAt).TotalMinutes <= 30)
            return user;
        return null;
    }

    public async Task InvalidateTemporaryPasswordAsync(int userId)
    {
        await _tempPasswordService.InvalidateTemporaryPasswordAsync(userId);
    }
}
