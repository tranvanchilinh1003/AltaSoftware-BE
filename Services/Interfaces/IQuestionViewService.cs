using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface IQuestionViewService
    {
        QuestionViewResponse GetViewCount(int questionId);
        void AddView(int questionId, int userId);
    }
}
