using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class ChangeClassRepo
    {
        private readonly isc_dbContext _context;
        public ChangeClassRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<ChangeClass> GetChangeClasses()
        {
            return _context.ChangeClasses.Where(s => s.Active == true).ToList();
        }

        public ChangeClass GetChangeClassById(long id)
        {
            return _context.ChangeClasses.FirstOrDefault(s => s.Id == id && s.Active == true);
        }

        public ChangeClass CreateChangeClass(ChangeClass ChangeClass)
        {
            _context.ChangeClasses.Add(ChangeClass);
            _context.SaveChanges();
            return ChangeClass;
        }

        public ChangeClass UpdateChangeClass(long id, ChangeClass ChangeClass)
        {
            // Lấy đối tượng ChangeClass hiện có từ CSDL theo id
            var existingChangeClass = _context.ChangeClasses.FirstOrDefault(s => s.Id == id);

            if (existingChangeClass == null)
            {
                // Xử lý trường hợp không tìm thấy, có thể ném exception hoặc trả về null
                return null;
            }


            existingChangeClass.StudentId = ChangeClass.StudentId;
            existingChangeClass.OldClassId = ChangeClass.OldClassId;
            existingChangeClass.ChangeClassDate = ChangeClass.ChangeClassDate;
            existingChangeClass.NewClassId = ChangeClass.NewClassId;
            existingChangeClass.Reason = ChangeClass.Reason;
            existingChangeClass.AttachmentName = ChangeClass.AttachmentName;
            existingChangeClass.AttachmentPath = ChangeClass.AttachmentPath;
            existingChangeClass.LeadershipId = ChangeClass.LeadershipId;
            existingChangeClass.Active = ChangeClass.Active;

            // Lưu các thay đổi xuống CSDL
            _context.SaveChanges();

            return existingChangeClass;
        }



        public bool DeleteChangeClass(long id)
        {
            var ChangeClass = GetChangeClassById(id);
            if (ChangeClass != null)
            {
                _context.ChangeClasses.Remove(ChangeClass);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
        public bool DeleteChangeClass2(long id)
        {
            var ChangeClass = GetChangeClassById(id);
            if (ChangeClass != null)
            {
                ChangeClass.Active = false;
                _context.ChangeClasses.Update(ChangeClass);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}
