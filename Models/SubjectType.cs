using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ISC_ELIB_SERVER.Models
{
    public partial class SubjectType
    {
        public SubjectType()
        {
            Subjects = new HashSet<Subject>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public bool? Status { get; set; }
        public string? Description { get; set; }
        public bool Active { get; set; }
        public int? AcademicYearsId { get; set; }

        public virtual AcademicYear AcademicYear { get; set; }
        [JsonIgnore]
        public virtual ICollection<Subject> Subjects { get; set; }
    }
}
