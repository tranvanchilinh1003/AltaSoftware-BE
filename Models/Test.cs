using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Test
    {
        public Test()
        {
            TestFiles = new HashSet<TestFile>();
            TestQuestions = new HashSet<TestQuestion>();
            TestsSubmissions = new HashSet<TestsSubmission>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Type { get; set; } // 0 trắc nghiệm, 1 tự luận
        public int? DurationTime { get; set; }
        public int? Classify { get; set; } // 0 KT 15p, 1 giữa kỳ 1, 2 cuối kỳ 1, 3 giữa kỳ 2, 4 cuối kỳ 2
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? File { get; set; }
        public string? Description { get; set; }
        public string? ClassIds { get; set; }
        public bool? FileSubmit { get; set; }
        public int? GradeLevelsId { get; set; }
        public int? SubjectId { get; set; }
        public int? UserId { get; set; }
        public bool? Active { get; set; }

        public virtual Subject? Subject { get; set; }
        public virtual User? User { get; set; }
        public virtual GradeLevel? GradeLevel { get; set; }
        public virtual ICollection<TestFile> TestFiles { get; set; }
        public virtual ICollection<TestQuestion> TestQuestions { get; set; }
        public virtual ICollection<TestsSubmission> TestsSubmissions { get; set; }
        [JsonIgnore]
        public virtual ICollection<TestUser> TestUsers { get; set; }
    }
}
