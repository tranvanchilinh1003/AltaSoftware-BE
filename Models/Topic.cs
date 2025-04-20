using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Topic
    {
        public Topic()
        {
            Discussions = new HashSet<Discussion>();
            TeachingAssignments = new HashSet<TeachingAssignment>();
            TopicsFiles = new HashSet<TopicsFile>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? EndDate { get; set; }
        public string? File { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<Discussion> Discussions { get; set; }
        public virtual ICollection<TeachingAssignment> TeachingAssignments { get; set; }
        public virtual ICollection<TopicsFile> TopicsFiles { get; set; }
    }
}
