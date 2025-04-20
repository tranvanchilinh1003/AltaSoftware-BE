using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class TestSubmissionAnswerAttachmentRepo
    {
        private readonly isc_dbContext _context;
        public TestSubmissionAnswerAttachmentRepo(isc_dbContext context)
        {
            _context = context;
        }
        public ICollection<TestSubmissionAnswerAttachment> GetAllTestSubmissionAnswerAttachments()
        {
            return _context.TestSubmissionAnswerAttachments.ToList();
        }
        public TestSubmissionAnswerAttachment GetTestSubmissionAnswerAttachmentById(long id)
        {
            return _context.TestSubmissionAnswerAttachments.FirstOrDefault(s => s.Id == id);
        }
        public TestSubmissionAnswerAttachment CreateTestSubmissionAnswerAttachment(TestSubmissionAnswerAttachment testSubmissionAnswerAttachment)
        {
            _context.TestSubmissionAnswerAttachments.Add(testSubmissionAnswerAttachment);
            _context.SaveChanges();
            return testSubmissionAnswerAttachment;
        }
    }
}
