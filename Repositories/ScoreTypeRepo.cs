using ISC_ELIB_SERVER.Models;
using Org.BouncyCastle.Crypto;

namespace ISC_ELIB_SERVER.Repositories
{
    public interface IScoreTypeRepo
    {
        ICollection<ScoreType> GetScoreTypes();
        ScoreType GetScoreTypeById(int id);
        ScoreType CreateScoreType(ScoreType scoreType);
        ScoreType UpdateScoreType(ScoreType scoreType);
        bool DeleteScoreType(int id);
    }

    public class ScoreTypeRepo : IScoreTypeRepo
    {
        private readonly isc_dbContext _context;

        public ScoreTypeRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<ScoreType> GetScoreTypes()
        {
            return _context.ScoreTypes.ToList();
        }

        public ScoreType GetScoreTypeById(int id)
        {
            return _context.ScoreTypes.FirstOrDefault(s => s.Id == id);
        }

        public ScoreType CreateScoreType(ScoreType scoreType)
        {
            _context.ScoreTypes.Add(scoreType);
            _context.SaveChanges();
            return scoreType;
        }

        public ScoreType? UpdateScoreType(ScoreType scoreType)
        {
            var existingScoreType = _context.ScoreTypes.Find(scoreType.Id);

            if (existingScoreType == null)
            {
                return null;
            }

            existingScoreType.Name = scoreType.Name;
            existingScoreType.Weight = scoreType.Weight;
            existingScoreType.QtyScoreSemester1 = scoreType.QtyScoreSemester1;
            existingScoreType.QtyScoreSemester2 = scoreType.QtyScoreSemester2;

            _context.SaveChanges();
            return existingScoreType;
        }


        public bool DeleteScoreType(int id)
        {
            var scoreType = _context.ScoreTypes.Find(id);

            if (scoreType == null)
            {
                return false;
            }

            scoreType.Active = false;
            _context.ScoreTypes.Update(scoreType);
            return _context.SaveChanges() > 0;
        }

    }
}
