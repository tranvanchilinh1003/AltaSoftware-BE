using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class AnswerImagesQaRepo
    {
        private readonly isc_dbContext _context;

        public AnswerImagesQaRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<AnswerImagesQa> GetAnswerImages()
        {
            return _context.AnswerImagesQas.Include(a => a.Answer).ToList();
        }

        public AnswerImagesQa GetAnswerImageById(long id)
        {
            return _context.AnswerImagesQas.Include(a => a.Answer).FirstOrDefault(a => a.Id == id);
        }

        public AnswerImagesQa CreateAnswerImage(AnswerImagesQa image)
        {
            _context.AnswerImagesQas.Add(image);
            _context.SaveChanges();
            return image;
        }

        public AnswerImagesQa UpdateAnswerImage(AnswerImagesQa image)
        {
            _context.AnswerImagesQas.Update(image);
            _context.SaveChanges();
            return image;
        }

        public bool DeleteAnswerImage(long id)
        {
            var image = GetAnswerImageById(id);
            if (image != null)
            {
                _context.AnswerImagesQas.Remove(image);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
