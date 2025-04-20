using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class TestAnswer
    {
        public TestAnswer()
        {
            TestSubmissionsAnswers = new HashSet<TestSubmissionsAnswer>();
        }

        public int Id { get; set; }
        public int? QuestionId { get; set; }
        public string? AnswerText { get; set; }
        public bool? IsCorrect { get; set; }
        public virtual TestQuestion? Question { get; set; }
        public virtual ICollection<TestSubmissionsAnswer> TestSubmissionsAnswers { get; set; }
    }
}
