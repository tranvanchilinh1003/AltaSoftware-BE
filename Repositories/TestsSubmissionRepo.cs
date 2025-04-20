using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class TestsSubmissionRepo
    {
        private readonly isc_dbContext _context;
        public TestsSubmissionRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<TestsSubmission> GetTestsSubmissions()
        {
            return _context.TestsSubmissions.ToList();
        }

        public TestsSubmission GetTestsSubmissionById(long id)
        {
            return _context.TestsSubmissions.FirstOrDefault(s => s.Id == id);
        }

        public TestsSubmission CreateTestsSubmission(TestsSubmission TestsSubmission)
        {
            _context.TestsSubmissions.Add(TestsSubmission);
            _context.SaveChanges();
            return TestsSubmission;
        }

        public TestsSubmission UpdateTestsSubmission(TestsSubmission TestsSubmission)
        {
            _context.TestsSubmissions.Update(TestsSubmission);
            _context.SaveChanges();
            return TestsSubmission;
        }

        public async Task<List<TestsSubmission>> GetByTestIdAsync(int testId)
        {
            return await _context.TestsSubmissions
                .Where(x => x.TestId == testId && x.Active == true)
                .Include(x => x.Student)
                .Include(x => x.Test)
                .Include(t => t.TestSubmissionsAnswers)
                    .ThenInclude(a => a.Attachments)
                .ToListAsync();
        }

        public bool DeleteTestsSubmission(long id)
        {
            var TestsSubmission = GetTestsSubmissionById(id);
            if (TestsSubmission != null)
            {
                _context.TestsSubmissions.Remove(TestsSubmission);
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

        public TestsSubmission GetTestSubmissionWithDetails(int submissionId)
        {
            // For Entity Framework Core
            return _context.TestsSubmissions
                .Include(x => x.Test)
                .Include(ts => ts.TestSubmissionsAnswers)
                    .ThenInclude(tsa => tsa.Attachments)
                .Where(ts => ts.Id == submissionId)
                .Where(ts => ts.Active == true)  // or ts.Active == 1 if it's an int
                .FirstOrDefault();
        }

        public List<TestsSubmission> GetSubmissionsByTestId(int testId)
        {
            return _context.TestsSubmissions
                .Include(x => x.Student) // nếu bạn muốn lấy thêm thông tin học sinh
                .Include(x => x.Test)    // nếu cần thông tin đề thi
                .Include(x => x.TestSubmissionsAnswers) // nếu cần danh sách câu trả lời
                .Where(x => x.TestId == testId)
                .Where(x => x.Active == true)
                .ToList();
        }

    }
}
