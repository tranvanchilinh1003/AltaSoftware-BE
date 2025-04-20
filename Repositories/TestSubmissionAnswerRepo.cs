using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class TestSubmissionAnswerRepo
    {
        private readonly isc_dbContext _context;
        public TestSubmissionAnswerRepo(isc_dbContext context)
        {
            _context = context;
        }
        public ICollection<TestSubmissionsAnswer> GetAllTestSubmissionAnswers()
        {
            return _context.TestSubmissionsAnswers.ToList();
        }
        public TestSubmissionsAnswer GetTestSubmissionAnswerById(long id)
        {
            return _context.TestSubmissionsAnswers.FirstOrDefault(s => s.Id == id);
        }
        public TestSubmissionsAnswer CreateTestSubmissionAnswer(TestSubmissionsAnswer testSubmissionAnswer)
        {
            _context.TestSubmissionsAnswers.Add(testSubmissionAnswer);
            _context.SaveChanges();
            return testSubmissionAnswer;
        }
        public TestSubmissionsAnswer UpdateTestSubmissionAnswer(TestSubmissionsAnswer entity)
        {
            _context.TestSubmissionsAnswers.Update(entity);
            _context.SaveChanges();
            return entity;
        }
        public bool DeleteTestSubmissionAnswer(long id)
        {
            var testSubmissionAnswer = GetTestSubmissionAnswerById(id);
            if (testSubmissionAnswer != null)
            {
                testSubmissionAnswer.Active = !testSubmissionAnswer.Active;
                return _context.SaveChanges() > 0;
            }
            return false;
        }

        public void AddAttachments(List<TestSubmissionAnswerAttachment> attachments)
        {
            _context.TestSubmissionAnswerAttachments.AddRange(attachments);
            _context.SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public ICollection<TestSubmissionAnswerAttachment> GetAttachmentsBySubmissionAnswerId(int id)
        {
            return _context.TestSubmissionAnswerAttachments
                           .Where(a => a.TestSubmissionAnswerId == id)
                           .ToList();
        }

        public ICollection<TestSubmissionAnswerAttachment> GetAttachmentsBySubmissionAnswerIds(List<int> answerIds)
        {
            return _context.TestSubmissionAnswerAttachments
                .Where(att => answerIds.Contains(att.TestSubmissionAnswerId))
                .ToList();
        }

        public void RemoveAttachments(List<TestSubmissionAnswerAttachment> attachments)
        {
            _context.TestSubmissionAnswerAttachments.RemoveRange(attachments);
            _context.SaveChanges();
        }

        //Lấy test_submission_answer theo test_id
        public ICollection<TestSubmissionsAnswer> GetAnswersByTestId(long testId)
        {
            return _context.TestSubmissionsAnswers
                           .Where(ans => _context.TestsSubmissions
                           .Any(sub => sub.Id == ans.SubmissionId && sub.TestId == testId))
                           .ToList();
        }
        public void DeleteAnswersBySubmissionId(int submissionId)
        {
            var answers = _context.TestSubmissionsAnswers
                .Where(a => a.SubmissionId == submissionId && a.Active)
                .ToList();

            if (answers.Any())
            {
                foreach (var answer in answers)
                {
                    answer.Active = false;

                    // Xoá luôn file đính kèm nếu có
                    var attachments = _context.TestSubmissionAnswerAttachments
                        .Where(att => att.TestSubmissionAnswerId == answer.Id)
                        .ToList();

                    _context.TestSubmissionAnswerAttachments.RemoveRange(attachments);
                }

                _context.SaveChanges(); // Ghi thay đổi
            }
        }

        public IEnumerable<TestSubmissionsAnswer> GetAnswersBySubmissionId(int submissionId)
        {
            return _context.TestSubmissionsAnswers
                .Include(a => a.Attachments) // nếu bạn có navigation property `Attachments`
                .Where(a => a.SubmissionId == submissionId && a.Active)
                .ToList();
        }

    }
}
