using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Achievement
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public DateTime? DateAwarded { get; set; }
        public string? File { get; set; }
        public int? UserId { get; set; }
        public int? TypeId { get; set; }
        public virtual User? User { get; set; }
    }
}
