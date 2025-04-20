using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class TrainingProgramsRepo
    {
        private readonly isc_dbContext _context;
        public TrainingProgramsRepo(isc_dbContext context)
        {
            _context = context;
        }

        public IQueryable<TeacherTrainingProgram> GetTeacherTrainingPrograms()
        {
            return _context.TeacherTrainingPrograms;
        }

        public IQueryable<TeacherInfo> GetTeacherInfo()
        {
            return _context.TeacherInfos;
        }

        public ICollection<TrainingProgram> GetTrainingProgram()
        {
            return _context.TrainingPrograms.ToList();
        }

        public TrainingProgram GetTrainingProgramById(long id)
        {
            return _context.TrainingPrograms.FirstOrDefault(s => s.Id == id);
        }

        public TrainingProgram CreateTrainingProgram(TrainingProgram trainingProgram)
        {
            _context.TrainingPrograms.Add(trainingProgram);
            _context.SaveChanges();
            return trainingProgram;
        }

        public TrainingProgram UpdateTrainingProgram(TrainingProgram trainingProgram)
        {
            _context.TrainingPrograms.Update(trainingProgram);
            _context.SaveChanges();
            return trainingProgram;
        }

        public bool DeleteTrainingProgram(TrainingProgram trainingProgram)
        {
            if (trainingProgram.StartDate.Kind != DateTimeKind.Utc)
            {
                trainingProgram.StartDate = trainingProgram.StartDate.ToUniversalTime();
            }
            if (trainingProgram.EndDate.Kind != DateTimeKind.Utc)
            {
                trainingProgram.EndDate = trainingProgram.EndDate.ToUniversalTime();
            }

            _context.TrainingPrograms.Update(trainingProgram);
            _context.SaveChanges();
            return true;
        }
    }
}
