using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using AutoMapper.Configuration.Annotations;

namespace ISC_ELIB_SERVER.Models
{
    public class TestSubmissionsAnswer
    {
        public int Id { get; set; }
        public int? SubmissionId { get; set; }
        public int? QuestionId { get; set; }
        public int? SelectedAnswerId { get; set; }
        public string? AnswerText { get; set; }
        public bool? IsCorrect { get; set; }
        public bool Active { get; set; }
        public double? Score { get; set; }
        public string? TeacherComment { get; set; }

        // Navigation properties
        public virtual TestQuestion? Question { get; set; }
        public virtual TestAnswer? SelectedAnswer { get; set; }
        public virtual TestsSubmission? Submission { get; set; }

        // One-to-many relationship to attachments
        public virtual ICollection<TestSubmissionAnswerAttachment> Attachments { get; set; } = new List<TestSubmissionAnswerAttachment>();
    }
}
