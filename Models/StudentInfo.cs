using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class StudentInfo
    {
        public int Id { get; set; }
        public string? GuardianName { get; set; }
        public string? GuardianPhone { get; set; }
        public string? GuardianJob { get; set; }
        public DateTime GuardianDob { get; set; }
        public string? GuardianAddress { get; set; }
        public string? GuardianRole { get; set; }
        public int? UserId { get; set; }
        public bool Active { get; set; }

        public virtual User? User { get; set; }  // Liên kết đến User
        public virtual ICollection<TransferSchool> TransferSchools { get; set; } // Liên kết đến TransferSchool
    }
}
