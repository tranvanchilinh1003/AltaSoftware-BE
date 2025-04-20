using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace ISC_ELIB_SERVER.Repositories
{
    public class AchivementRepo
    {
        private readonly isc_dbContext _context;
        public AchivementRepo(isc_dbContext context)
        {
            _context = context;
        }
        public IQueryable<Achievement> GetAchievements()
        {
            return _context.Achievements
                .Include(a => a.User);
        }

        public IQueryable<Achievement> GetAchievementsByTypeId(int typeId)
        {
            return _context.Achievements
                .Include(a => a.User)
                .Where(a => a.TypeId == typeId);
        }

        public Achievement? GetAchivementById(int id)
        {

            return _context.Achievements
                .Include(a => a.User)
                .FirstOrDefault(s => s.Id == id);
        }

        public Achievement CreateAchivement(Achievement achivement)
        {
            _context.Achievements.Add(achivement);
            _context.SaveChanges();

            var result = _context.Achievements
                                 .Include(a => a.User)
                                 .FirstOrDefault(a => a.Id == achivement.Id);

            return result ?? throw new Exception("Không thể lấy dữ liệu thành tích vừa tạo.");
        }


        public Achievement? UpdateAchivement(Achievement updatedAchivement)
        {
            _context.Achievements.Update(updatedAchivement);
            _context.SaveChanges();

            var result = _context.Achievements
                                 .Include(a => a.User)
                                 .FirstOrDefault(a => a.Id == updatedAchivement.Id);

            return result;
        }


        public bool DeleteAchivement(int id)
        {
            var Achivement = GetAchivementById(id);
            if (Achivement != null)
            {
                _context.Achievements.Remove(Achivement);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}
