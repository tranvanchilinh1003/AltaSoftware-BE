using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISC_ELIB_SERVER.Models
{
    [Table("question_views")]
    public class QuestionView
    {
        public int Id { get; set; } // Khóa chính
        public int QuestionId { get; set; } // ID của câu hỏi
        public int UserId { get; set; } // ID của người dùng đã xem

        [Column("viewed_at", TypeName = "timestamp")]
        public DateTime ViewedAt { get; set; } = DateTime.UtcNow; // Thời gian xem

        [ForeignKey("QuestionId")]
        public virtual QuestionQa Question { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }

}
