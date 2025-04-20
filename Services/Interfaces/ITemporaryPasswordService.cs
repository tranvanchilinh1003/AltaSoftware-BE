using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface ITemporaryPasswordService
    {
        Task<TemporaryPasswordRecord?> GetByUserIdAsync(int userId);
        Task SaveTemporaryPasswordAsync(TemporaryPasswordRecord record);
        Task InvalidateTemporaryPasswordAsync(int userId);

    }
}
