using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class ResignationRepo
    {
        private readonly isc_dbContext _context;
        public ResignationRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<Resignation> GetResignation()
        {
            return _context.Resignations.ToList();
        }

        public Resignation GetResignationById(long id)
        {
            return _context.Resignations.FirstOrDefault(s => s.Id == id);
        }

        public ICollection<Resignation> GetResignationsByTeacherId(long id)
        {
            return _context.Resignations
                .Where(s => s.TeacherId == id)
                .ToList();
        }

        public Resignation CreateResignation(Resignation Resignation)
        {
            _context.Resignations.Add(Resignation);
            _context.SaveChanges();
            return Resignation;
        }

        public Resignation UpdateResignation(long id, ResignationRequest Resignation)
        {
            var existingResignation = GetResignationById(id);


            if (existingResignation == null)
            {
                return null;
            }
            existingResignation.Date = Resignation.Date;
            existingResignation.Note = Resignation.Note;
            existingResignation.Attachment = Resignation.Attachment;
            existingResignation.Status = Resignation.Status;
            existingResignation.Active = Resignation.Active;
            _context.Resignations.Update(existingResignation);

            _context.SaveChanges();

            return existingResignation;
        }


        public bool DeleteResignation(long id)
        {
            var Resignation = GetResignationById(id);
            if (Resignation != null)
            {
                _context.Resignations.Remove(Resignation);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}
