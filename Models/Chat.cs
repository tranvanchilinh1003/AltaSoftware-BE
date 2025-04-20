using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Chat
    {
        public int Id { get; set; }
        public DateTime? SentAt { get; set; }
        public string? Content { get; set; }
        public int? UserId { get; set; }
        public int? SessionId { get; set; }
        public bool Active { get; set; }

        public virtual Session? Session { get; set; }
    }
}
