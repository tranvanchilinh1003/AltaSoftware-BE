using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;

namespace ISC_ELIB_SERVER.Services
{
    public interface IQuestionImagesQaService
    {
        ApiResponse<ICollection<QuestionImagesQaResponse>> GetQuestionImages(long? questionId);
        ApiResponse<QuestionImagesQaResponse> GetQuestionImageById(long id);
        ApiResponse<QuestionImagesQaResponse> CreateQuestionImage(QuestionImagesQaRequest request);
        ApiResponse<QuestionImagesQaResponse> UpdateQuestionImage(long id, QuestionImagesQaRequest request);
        ApiResponse<QuestionImagesQaResponse> DeleteQuestionImage(long id);
    }

  
}
