using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISC_ELIB_SERVER.Models
{
    public partial class AnswersQa
    {
        public AnswersQa()
        {
            AnswerImagesQas = new HashSet<AnswerImagesQa>();
        }

        public int Id { get; set; }
        public int? QuestionId { get; set; }
        public int? UserId { get; set; }
        public string? Content { get; set; }
        public DateTime? CreateAt { get; set; }
        [Column("active")]
        public bool Active { get; set; } = true;

        public virtual QuestionQa? Question { get; set; }
        public virtual ICollection<AnswerImagesQa> AnswerImagesQas { get; set; }

        // 🔥 Thêm quan hệ với bảng Users
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}
