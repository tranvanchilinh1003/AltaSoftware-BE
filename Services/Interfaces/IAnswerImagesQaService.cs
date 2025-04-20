using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;

namespace ISC_ELIB_SERVER.Services
{
    public interface IAnswerImagesQaService
    {
        ApiResponse<ICollection<AnswerImagesQaResponse>> GetAnswerImages(long? answerId);
        ApiResponse<AnswerImagesQaResponse> GetAnswerImageById(long id);
        ApiResponse<AnswerImagesQaResponse> CreateAnswerImage(AnswerImagesQaRequest request);
        ApiResponse<AnswerImagesQaResponse> UpdateAnswerImage(long id, AnswerImagesQaRequest request);
        ApiResponse<AnswerImagesQaResponse> DeleteAnswerImage(long id);
    }

  
}
