using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Exemption
    {
        public int Id { get; set; }
        public int? StudentId { get; set; }
        public int? ClassId { get; set; }
        public string? ExemptedObjects { get; set; }
        public string? FormExemption { get; set; }
        public bool Active { get; set; }

        public virtual Class? Class { get; set; }
        public virtual User? Student { get; set; }
    }
}
