using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class TemporaryLeaveRepo
    {
        private readonly isc_dbContext _context;
        public TemporaryLeaveRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<TemporaryLeave> GetTemporaryLeaves()
        {
            return _context.TemporaryLeaves.Where(d => d.Active == true).ToList();
        }

        public TemporaryLeave GetTemporaryLeaveById(long id)
        {
            return _context.TemporaryLeaves.FirstOrDefault(s => s.Id == id && s.Active == true);
        }

        public TemporaryLeave CreateTemporaryLeave(TemporaryLeave TemporaryLeave)
        {
            _context.TemporaryLeaves.Add(TemporaryLeave);
            _context.SaveChanges();
            return TemporaryLeave;
        }

        public TemporaryLeave UpdateTemporaryLeave(long id, TemporaryLeave temporaryLeave)
        {
            // Lấy đối tượng TemporaryLeave hiện có từ CSDL theo id
            var existingTemporaryLeave = _context.TemporaryLeaves.FirstOrDefault(s => s.Id == id);

            if (existingTemporaryLeave == null)
            {
                // Xử lý trường hợp không tìm thấy, có thể ném exception hoặc trả về null
                return null;
            }

            // Cập nhật các thuộc tính được phép thay đổi (không cập nhật Id)
            existingTemporaryLeave.Date = temporaryLeave.Date;
            existingTemporaryLeave.Note = temporaryLeave.Note;
            existingTemporaryLeave.Attachment = temporaryLeave.Attachment;
            existingTemporaryLeave.Status = temporaryLeave.Status;
            existingTemporaryLeave.TeacherId = temporaryLeave.TeacherId;
            existingTemporaryLeave.LeadershipId = temporaryLeave.LeadershipId;
            existingTemporaryLeave.Active = temporaryLeave.Active;
            // Lưu các thay đổi xuống CSDL
            _context.SaveChanges();

            return existingTemporaryLeave;
        }



        public bool DeleteTemporaryLeave(long id)
        {
            var TemporaryLeave = GetTemporaryLeaveById(id);
            if (TemporaryLeave != null)
            {
                _context.TemporaryLeaves.Remove(TemporaryLeave);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
        public bool DeleteTemporaryLeave2(long id)
        {
            var temporaryLeave = GetTemporaryLeaveById(id);
            if (temporaryLeave != null)
            {
                temporaryLeave.Active = false;
                _context.TemporaryLeaves.Update(temporaryLeave);
                return _context.SaveChanges() > 0;
            }
            return false;
        }

    }
}
