using ISC_ELIB_SERVER.Models;
using System.Collections.Generic;
using System.Linq;

namespace ISC_ELIB_SERVER.Repositories
{
    public class EntryTypeRepo
    {
        private readonly isc_dbContext _context;

        public EntryTypeRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<EntryType> GetEntryTypes()
        {
            return _context.EntryTypes
                .Where(e => e.Active) // Chỉ lấy các bản ghi đang hoạt động
                .ToList();
        }

        public EntryType? GetEntryTypeById(long id)
        {
            return _context.EntryTypes.FirstOrDefault(e => e.Id == id && e.Active);
        }

        public EntryType CreateEntryType(EntryType entryType)
        {
            entryType.Active = true; // Mặc định dữ liệu mới là đang hoạt động
            _context.EntryTypes.Add(entryType);
            _context.SaveChanges();
            return entryType;
        }

        public EntryType? UpdateEntryType(EntryType entryType)
        {
            var existingEntry = GetEntryTypeById(entryType.Id);
            if (existingEntry == null) return null;

            _context.EntryTypes.Update(entryType);
            _context.SaveChanges();
            return entryType;
        }


        public bool DeactivateEntryType(long id)
        {
            var entryType = _context.EntryTypes.FirstOrDefault(e => e.Id == id);
            if (entryType == null) return false;

            entryType.Active = false; // Đánh dấu là không hoạt động
            _context.EntryTypes.Update(entryType);
            return _context.SaveChanges() > 0;
        }
    }
}
