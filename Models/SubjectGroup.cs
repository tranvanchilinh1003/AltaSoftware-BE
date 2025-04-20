using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ISC_ELIB_SERVER.Models
{
    public partial class SubjectGroup
    {
        public SubjectGroup()
        {
            Subjects = new HashSet<Subject>();
            WorkProcesses = new HashSet<WorkProcess>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public int? TeacherId { get; set; }
        public bool Active { get; set; }

        public virtual User? Teacher { get; set; }
        public virtual ICollection<WorkProcess> WorkProcesses { get; set; }
        [JsonIgnore]
        public virtual ICollection<Subject> Subjects { get; set; }
    }
}
