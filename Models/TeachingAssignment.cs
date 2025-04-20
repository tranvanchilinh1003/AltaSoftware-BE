using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class TeachingAssignment
    {
        public TeachingAssignment()
        {
            Sessions = new HashSet<Session>();
        }

        public int Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Description { get; set; }
        public int? UserId { get; set; }
        public int? ClassId { get; set; }
        public int? SubjectId { get; set; }
        public int? TopicsId { get; set; }
        public int? SemesterId { get; set; }
        public bool Active { get; set; }

        public virtual Class? Class { get; set; }
        public virtual Subject? Subject { get; set; }
        public virtual Topic? Topics { get; set; }
        public virtual User? User { get; set; }
        public virtual Semester? Semester { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
