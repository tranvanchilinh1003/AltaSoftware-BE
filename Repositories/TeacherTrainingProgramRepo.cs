using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class TeacherTrainingProgramRepo
    {
        private readonly isc_dbContext _context;
        public TeacherTrainingProgramRepo(isc_dbContext context)
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

        //public TeacherTrainingProgram GetTeacherTrainingProgramById(long id)
        //{
        //    return _context.TeacherTrainingPrograms.FirstOrDefault(s => s.id == id);
        //}

        public TeacherTrainingProgram CreateTeacherTrainingPrograms(TeacherTrainingProgram teacherTrainingProgram)
        {
            _context.TeacherTrainingPrograms.Add(teacherTrainingProgram);
            _context.SaveChanges();
            return teacherTrainingProgram;
        }

        public TeacherTrainingProgram UpdateTrainingProgram(TeacherTrainingProgram teacherTrainingProgram)
        {
            _context.TeacherTrainingPrograms.Update(teacherTrainingProgram);
            _context.SaveChanges();
            return teacherTrainingProgram;
        }

        //public bool DeleteTrainingProgram(TrainingProgram trainingProgram)
        //{
        //    if (trainingProgram.StartDate.Kind != DateTimeKind.Utc)
        //    {
        //        trainingProgram.StartDate = trainingProgram.StartDate.ToUniversalTime();
        //    }
        //    if (trainingProgram.EndDate.Kind != DateTimeKind.Utc)
        //    {
        //        trainingProgram.EndDate = trainingProgram.EndDate.ToUniversalTime();
        //    }

        //    _context.TrainingPrograms.Update(trainingProgram);
        //    _context.SaveChanges();
        //    return true;
        //}
    }
}
