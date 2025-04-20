using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISC_ELIB_SERVER.Models
{
    public partial class ExamScheduleClass
    {
        public int Id { get; set; }
        public int? ClassId { get; set; }
        public int? ExampleSchedule { get; set; }
        public int? SupervisoryTeacherId { get; set; }
        public int joined_student_quantity { get; set; }
        public bool Active { get; set; }


        public virtual ICollection<ExamGrader> ExamGraders { get; set; } = new HashSet<ExamGrader>();
        public virtual Class? Class { get; set; }
        public virtual ExamSchedule? ExampleScheduleNavigation { get; set; }
        public virtual TeacherInfo? SupervisoryTeacher { get; set; }
    }
}
