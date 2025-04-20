using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class TeacherTrainingProgram
    {
        public int TeacherId { get; set; }
        public int TrainingProgramId { get; set; }

        public virtual TeacherInfo Teacher { get; set; } = null!;
    }
}
