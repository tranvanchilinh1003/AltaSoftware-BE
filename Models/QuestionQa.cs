using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISC_ELIB_SERVER.Models
{
    [Table("question_qa")]
    public partial class QuestionQa
    {
        public QuestionQa()
        {
            AnswersQas = new HashSet<AnswersQa>();
            QuestionImagesQas = new HashSet<QuestionImagesQa>();
        }

        public int Id { get; set; }
        public int? UserId { get; set; } // ID người đặt câu hỏi
        public int? SubjectId { get; set; }
        public string? Content { get; set; }
         [Column("create_at")]
        public DateTime? CreateAt { get; set; }
        [Column("active")] 
        public bool Active { get; set; } = true; 

        // Thêm thuộc tính navigation tới User
        [ForeignKey("UserId")]
        public virtual User? User { get; set; } 

        public virtual Subject? Subject { get; set; }

        public virtual ICollection<AnswersQa> AnswersQas { get; set; }
        public virtual ICollection<QuestionImagesQa> QuestionImagesQas { get; set; }
    }
}
