using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;
using ISC_ELIB_SERVER.Services.Interfaces;

namespace ISC_ELIB_SERVER.Services
{


    public class ExemptionService : IExemptionService
    {
        private readonly ExemptionRepo _repository;
        private readonly IMapper _mapper;

        public ExemptionService(ExemptionRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<ExemptionResponse>> GetExemptions(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetExemptions().AsQueryable();

            query = sortColumn switch
            {
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                _ => query.OrderBy(us => us.Id)
            };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var response = _mapper.Map<ICollection<ExemptionResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<ExemptionResponse>>.Success(response)
                : ApiResponse<ICollection<ExemptionResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<ICollection<ExemptionResponse>> GetExemptionsNormal()
        {
            var result = _repository.GetExemptions();

            var response = _mapper.Map<ICollection<ExemptionResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<ExemptionResponse>>.Success(response)
                : ApiResponse<ICollection<ExemptionResponse>>.NotFound("Không có dữ liệu");
        }


        public ApiResponse<ExemptionResponse> GetExemptionById(long id)
        {
            var Exemption = _repository.GetExemptionById(id);
            return Exemption != null
                ? ApiResponse<ExemptionResponse>.Success(_mapper.Map<ExemptionResponse>(Exemption))
                : ApiResponse<ExemptionResponse>.NotFound($"Không tìm thấy thông tin tạm nghỉ của #{id}");
        }


        public ApiResponse<ExemptionResponse> CreateExemption(Exemption_AddRequest ExemptionRequest)
        {

            var Exemption = _mapper.Map<Exemption>(ExemptionRequest);
            var created = _repository.CreateExemption(Exemption);
            return ApiResponse<ExemptionResponse>.Success(_mapper.Map<ExemptionResponse>(created));
        }

        public ApiResponse<Exemption> UpdateExemption(long id , Exemption_UpdateRequest ExemptionRequest)
        {
            var Exemption = _mapper.Map<Exemption>(ExemptionRequest);
            var updated = _repository.UpdateExemption(id ,Exemption);
            return updated != null
                ? ApiResponse<Exemption>.Success(updated)
                : ApiResponse<Exemption>.NotFound("Không tìm thấy trạng thái người dùng để cập nhật");
        }

        public ApiResponse<Exemption> DeleteExemption(long id)
        {
            var success = _repository.DeleteExemption2(id);
            return success
                ? ApiResponse<Exemption>.Success()
                : ApiResponse<Exemption>.NotFound("Không tìm thấy trạng thái người dùng để xóa");
        }
    }

}
