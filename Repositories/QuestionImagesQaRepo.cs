using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class QuestionImagesQaRepo
    {
        private readonly isc_dbContext _context;

        public QuestionImagesQaRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<QuestionImagesQa> GetQuestionImages()
        {
            return _context.QuestionImagesQas.Include(q => q.Question).ToList();
        }

        public QuestionImagesQa GetQuestionImageById(long id)
        {
            return _context.QuestionImagesQas.Include(q => q.Question).FirstOrDefault(q => q.Id == id);
        }

        public QuestionImagesQa CreateQuestionImage(QuestionImagesQa image)
        {
            _context.QuestionImagesQas.Add(image);
            _context.SaveChanges();
            return image;
        }

        public QuestionImagesQa UpdateQuestionImage(QuestionImagesQa image)
        {
            _context.QuestionImagesQas.Update(image);
            _context.SaveChanges();
            return image;
        }

        public bool DeleteQuestionImage(long id)
        {
            var image = GetQuestionImageById(id);
            if (image != null)
            {
                _context.QuestionImagesQas.Remove(image);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
