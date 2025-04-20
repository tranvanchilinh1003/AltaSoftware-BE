using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using AutoMapper;
using ISC_ELIB_SERVER.Services.Interfaces;

namespace ISC_ELIB_SERVER.Services
{
    public class QuestionViewService : IQuestionViewService
    {
        private readonly QuestionViewRepo _repository;
        private readonly IMapper _mapper;

        public QuestionViewService(QuestionViewRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // Lấy số lượt xem
        public QuestionViewResponse GetViewCount(int questionId)
        {
            var viewCount = _repository.GetViewCount(questionId);
            return new QuestionViewResponse { QuestionId = questionId, ViewCount = viewCount };
        }

        // Thêm lượt xem nếu chưa có
        public void AddView(int questionId, int userId)
        {
            _repository.AddView(questionId, userId);
        }

    }
}
