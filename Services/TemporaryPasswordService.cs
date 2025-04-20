using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Services.Interfaces;
using System.Collections.Concurrent;

namespace ISC_ELIB_SERVER.Services
{
    public class TemporaryPasswordService : ITemporaryPasswordService
    {
        private readonly ConcurrentDictionary<int, TemporaryPasswordRecord> _store = new();

        public Task<TemporaryPasswordRecord?> GetByUserIdAsync(int userId)
        {
            _store.TryGetValue(userId, out var record);
            return Task.FromResult(record);
        }

        public Task SaveTemporaryPasswordAsync(TemporaryPasswordRecord record)
        {
            _store[record.UserId] = record;
            return Task.CompletedTask;
        }

        public Task InvalidateTemporaryPasswordAsync(int userId)
        {
            _store.TryRemove(userId, out _);
            return Task.CompletedTask;
        }
    }
}
