using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Campus
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public int? SchoolId { get; set; }
        public int? UserId { get; set; }
        public bool Active { get; set; }

        // public virtual School? School { get; set; }
        public virtual User? User { get; set; }
    }
}
