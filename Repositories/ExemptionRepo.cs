using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class ExemptionRepo
    {
        private readonly isc_dbContext _context;
        public ExemptionRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<Exemption> GetExemptions()
        {
            return _context.Exemptions.Where(s => s.Active == true).ToList();
        }

        public Exemption GetExemptionById(long id)
        {
            return _context.Exemptions.FirstOrDefault(s => s.Id == id && s.Active == true);
        }

        public Exemption CreateExemption(Exemption Exemption)
        {
            _context.Exemptions.Add(Exemption);
            _context.SaveChanges();
            return Exemption;
        }

        public Exemption UpdateExemption(long id, Exemption Exemption)
        {
            // Lấy đối tượng Exemption hiện có từ CSDL theo id
            var existingExemption = _context.Exemptions.FirstOrDefault(s => s.Id == id);

            if (existingExemption == null)
            {
                // Xử lý trường hợp không tìm thấy, có thể ném exception hoặc trả về null
                return null;
            }
            existingExemption.StudentId = Exemption.StudentId;
            existingExemption.ClassId = Exemption.ClassId;
            existingExemption.ExemptedObjects = Exemption.ExemptedObjects;
            existingExemption.FormExemption = Exemption.FormExemption;
            existingExemption.Active = Exemption.Active;

            // Lưu các thay đổi xuống CSDL
            _context.SaveChanges();

            return existingExemption;
        }



        public bool DeleteExemption(long id)
        {
            var Exemption = GetExemptionById(id);
            if (Exemption != null)
            {
                _context.Exemptions.Remove(Exemption);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
        public bool DeleteExemption2(long id)
        {
            var Exemption = GetExemptionById(id);
            if (Exemption != null)
            {
                Exemption.Active = false;
                _context.Exemptions.Update(Exemption);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}
