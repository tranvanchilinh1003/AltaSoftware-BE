using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISC_ELIB_SERVER.Models
{
    public partial class AnswerImagesQa
    {
        public int Id { get; set; }
        public int? AnswerId { get; set; }
        public string? ImageUrl { get; set; }
        [Column("active")]
        public bool Active { get; set; } = true;

        public virtual AnswersQa? Answer { get; set; }
    }
}
