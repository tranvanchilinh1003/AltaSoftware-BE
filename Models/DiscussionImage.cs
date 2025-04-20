using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class DiscussionImage
    {
        public int Id { get; set; }
        public int? DiscussionId { get; set; }
        public string? ImageUrl { get; set; }
        public bool Active { get; set; }

        public virtual Discussion? Discussion { get; set; }
    }
}
