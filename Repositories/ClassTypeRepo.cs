using System.Linq.Dynamic.Core;
using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public interface IClassTypeRepo
    {
        ICollection<ClassType> GetClassTypes();
        ClassType GetClassTypeById(int id);
        ClassType CreateClassType(ClassType classType);
        ClassType? UpdateClassType(ClassType classType);
        bool DeleteClassType(int id);
    }

    public class ClassTypeRepo : IClassTypeRepo
    {
        private readonly isc_dbContext _context;

        public ClassTypeRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<ClassType> GetClassTypes()
        {
            return _context.ClassTypes
                .AsNoTracking()
                .Where(ct => ct.Active)
                .Include(ct => ct.AcademicYear)
                .ToList();
        }


        public ClassType? GetClassTypeById(int id)
        {
            return _context.ClassTypes
                .AsNoTracking()
                .Where(ct => ct.Active)
                .Include(ct => ct.AcademicYear)
                .FirstOrDefault(ct => ct.Id == id);
        }


        public ClassType CreateClassType(ClassType classType)
        {
            _context.ClassTypes.Add(classType);
            _context.SaveChanges();

            return _context.ClassTypes
                .AsNoTracking()
                .Include(ct => ct.AcademicYear)
                .FirstOrDefault(ct => ct.Id == classType.Id);
        }


        public ClassType? UpdateClassType(ClassType classType)
        {
            var existingClassType = _context.ClassTypes
                .AsNoTracking()
                .FirstOrDefault(ct => ct.Id == classType.Id);

            if (existingClassType == null)
            {
                return null; 
            }

            _context.Entry(classType).State = EntityState.Modified;
            _context.SaveChanges();

            return _context.ClassTypes
                .AsNoTracking()
                .Include(ct => ct.AcademicYear)
                .FirstOrDefault(ct => ct.Id == classType.Id);
        }

        public bool DeleteClassType(int id)
        {
            var classType = _context.ClassTypes.Find(id);
            if (classType == null)
            {
                return false;
            }

            bool hasForeignKeyReferences = _context.Classes.Any(c => c.ClassTypeId == id)
                ||_context.Exams.Any(e => e.ClassTypeId == id)||_context.ClassTypes.Any(ct => ct.AcademicYearId == id);

            if (hasForeignKeyReferences)
            {
                classType.Active = true;
                _context.ClassTypes.Update(classType);
            }
            else
            {
                _context.ClassTypes.Remove(classType);
            }

            return _context.SaveChanges() > 0;
        }
    }
}
