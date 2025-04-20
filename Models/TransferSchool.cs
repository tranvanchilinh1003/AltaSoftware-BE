using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISC_ELIB_SERVER.Models
{
    public partial class TransferSchool
    {
        public int Id { get; set; }
        public int? StudentId { get; set; }


        public DateTime? TransferSchoolDate { get; set; }
        public string? TransferToSchool { get; set; }
        public string? SchoolAddress { get; set; }
        public string? Reason { get; set; }
        public string? AttachmentName { get; set; }
        public string? AttachmentPath { get; set; }



        [Column("semester_id")]  // Chỉ định tên cột đúng trong PostgreSQL
        public int? SemesterId { get; set; }
        public int? UserId { get; set; } 
        public bool Active { get; set; }

        public virtual StudentInfo? Student { get; set; } // Liên kết đến StudentInfo

        public virtual User? User { get; set; } // Liên kết đến Semester

        public virtual Semester? Semester  { get; set; } // Liên kết đến Semester
      
    }
}
