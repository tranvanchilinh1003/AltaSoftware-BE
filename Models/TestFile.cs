using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class TestFile
    {
        public int Id { get; set; }
        public int? TestId { get; set; }
        public string? FileUrl { get; set; }
        public bool Active { get; set; }

        public virtual Test? Test { get; set; }
    }
}
