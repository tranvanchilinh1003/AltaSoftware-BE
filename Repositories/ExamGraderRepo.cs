using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class ExamGraderRepo
    {
        private readonly isc_dbContext _context;

        public ExamGraderRepo(isc_dbContext context)
        {
            _context = context;
        }

        public PagedResult<ExamGrader> GetAll(int page, int pageSize, string? search, string? sortBy, bool isDescending)
        {
            var query = _context.ExamGraders
        .Include(e => e.User) 
        .AsQueryable();

            // 🔍 Tìm kiếm theo `UserId` hoặc `ExamId`
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e =>
                    e.UserId.ToString().Contains(search) ||
                    e.ExamId.ToString().Contains(search));
            }

            // 🔄 Sắp xếp động
            if (!string.IsNullOrEmpty(sortBy))
            {
                query = isDescending
                    ? query.OrderByDescending(e => EF.Property<object>(e, sortBy))
                    : query.OrderBy(e => EF.Property<object>(e, sortBy));
            }

            // 📌 Tổng số bản ghi
            var totalCount = query.Count();

            // ⏳ Phân trang
            var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PagedResult<ExamGrader>(items, totalCount, page, pageSize);
        }
        public ExamGrader? GetById(long id)
        {
            return _context.ExamGraders.FirstOrDefault(e => e.Id == id);
        }

        public ExamGrader Create(ExamGrader examGrader)
        {
            _context.ExamGraders.Add(examGrader);
            _context.SaveChanges();
            return examGrader;
        }

        public ExamGrader? Update(ExamGrader examGrader)
        {
            _context.ExamGraders.Update(examGrader);
            _context.SaveChanges();
            return examGrader;
        }

        public bool Delete(long id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _context.ExamGraders.Remove(entity);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}
