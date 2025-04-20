using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class TopicsFile
    {
        public int Id { get; set; }
        public int? TopicId { get; set; }
        public string? FileUrl { get; set; }
        public string? FileName { get; set; }
        public DateTime? CreateAt { get; set; }
        public bool Active { get; set; }

        public virtual Topic? Topic { get; set; }
    }
}
