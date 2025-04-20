using ISC_ELIB_SERVER.Models;

public static class UserExtensions
{
    private static readonly Dictionary<int, string?> Otps = new();
    private static readonly Dictionary<int, DateTime?> OtpExpirations = new();

    public static string? GetOtp(this User user)
    {
        if (Otps.TryGetValue(user.Id, out var otp))
        {
            return otp;
        }
        return null;
    }

    public static void SetOtp(this User user, string? otp)
    {
        Otps[user.Id] = otp;
    }

    public static DateTime? GetOtpExpiration(this User user)
    {
        if (OtpExpirations.TryGetValue(user.Id, out var expiration))
        {
            return expiration;
        }
        return null;
    }

    public static void SetOtpExpiration(this User user, DateTime? expiration)
    {
        OtpExpirations[user.Id] = expiration;
    }
}
