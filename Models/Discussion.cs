using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Discussion
    {
        public Discussion()
        {
            DiscussionImages = new HashSet<DiscussionImage>();
        }

        public int Id { get; set; }
        public int? TopicId { get; set; }
        public int? UserId { get; set; }
        public string? Content { get; set; }
        public DateTime? CreateAt { get; set; }
        public bool Active { get; set; }

        public virtual Topic? Topic { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<DiscussionImage> DiscussionImages { get; set; }
    }
}
