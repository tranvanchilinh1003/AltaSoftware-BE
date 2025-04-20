using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Support
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public int? Type { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? UserId { get; set; }
        public bool Active { get; set; }

        public virtual User? User { get; set; }
    }
}
