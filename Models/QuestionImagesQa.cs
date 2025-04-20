using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class QuestionImagesQa
    {
        public int Id { get; set; }
        public int? QuestionId { get; set; }
        public string? ImageUrl { get; set; }
        public bool Active { get; set; }

        public virtual QuestionQa? Question { get; set; }
    }
}
