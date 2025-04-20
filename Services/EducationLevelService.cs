using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;
using ISC_ELIB_SERVER.Services.Interfaces;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ISC_ELIB_SERVER.Services
{

    public class EducationLevelService : IEducationLevelService
    {
        private readonly EducationLevelRepo _repository;
        private readonly IMapper _mapper;

        public EducationLevelService(EducationLevelRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<EducationLevelResponse>> GetEducationLevels(int? page, int? pageSize, string? sortColumn, string? sortOrder)
        {
            var query = _repository.GetEducationLevels().AsQueryable();

            query = sortColumn switch
            {
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                _ => query.OrderBy(ay => ay.Id)
            };


            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            var result = query.ToList();

            var response = _mapper.Map<ICollection<EducationLevelResponse>>(result);

            return result.Any() ? ApiResponse<ICollection<EducationLevelResponse>>.Success(response, page, pageSize, _repository.GetEducationLevels().Count) : ApiResponse<ICollection<EducationLevelResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<EducationLevelResponse> GetEducationLevelById(long id)
        {
            var EducationLevel = _repository.GetEducationLevelById(id);
            return EducationLevel != null
                ? ApiResponse<EducationLevelResponse>.Success(_mapper.Map<EducationLevelResponse>(EducationLevel))
                : ApiResponse<EducationLevelResponse>.NotFound($"Không tìm thấy cấp bậc đào tạo #{id}");
        }

        public ApiResponse<EducationLevelResponse> CreateEducationLevel(EducationLevelRequest EducationLevelRequest)
        {
            //var ListEducationLevel = _repository.GetEducationLevels();


            //if (EducationLevelRequest.IsAnnualSystem == true && EducationLevelRequest.IsCredit == true)
            //{
            //    return ApiResponse<EducationLevelResponse>.BadRequest("Không thể áp dụng đồng thời cả hệ niên chế và hệ tín chỉ");
            //}

            //if (EducationLevelRequest.IsAnnualSystem == false && EducationLevelRequest.IsCredit == false)
            //{
            //    return ApiResponse<EducationLevelResponse>.BadRequest("Vui lòng chọn 1 trong 2 hệ niên chế hoặc tín chỉ");
            //}

            var newEducationLevel = new EducationLevel
            {
                Name = EducationLevelRequest.Name,
                TrainingType = EducationLevelRequest.TrainingType,
                IsAnnualSystem = EducationLevelRequest.IsAnnualSystem,
                IsCredit = EducationLevelRequest.IsCredit,
                Status = EducationLevelRequest.Status,
                Description = EducationLevelRequest.Description,
                TrainingDuration = EducationLevelRequest.TrainingDuration,
                SemesterPerYear = EducationLevelRequest.SemesterPerYear,
                MandatoryCourse = EducationLevelRequest.MandatoryCourse,
                ElectiveCourse = EducationLevelRequest.ElectiveCourse
            };


            try
            {
                var created = _repository.CreateEducationLevel(newEducationLevel);
                return ApiResponse<EducationLevelResponse>.Success(_mapper.Map<EducationLevelResponse>(created));
            }
            catch (DbUpdateException ex)
            {
                return ApiResponse<EducationLevelResponse>.BadRequest("Lỗi khi thêm cấp bậc đào tạo: " + ex.InnerException?.Message);
            }
            catch (Exception ex)
            {
                return ApiResponse<EducationLevelResponse>.BadRequest("Lỗi khi thêm cấp bậc đào tạo: " + ex.Message);
            }

        }

        public ApiResponse<EducationLevelResponse> UpdateEducationLevel(long id, EducationLevelRequest EducationLevelRequest)
        {
            var existing = _repository.GetEducationLevelById(id);
            if (existing == null)
            {
                return ApiResponse<EducationLevelResponse>.NotFound($"Không tìm thấy cấp bậc đào tạo #{id}");
            }

            //var ListEducationLevel = _repository.GetEducationLevels().Where(item => item.Active); ;

            //if (ListEducationLevel.Any(item => item.Name.Equals(EducationLevelRequest.Name) && item.Id != id))
            //{
            //    return ApiResponse<EducationLevelResponse>.BadRequest("Tên cấp bậc đào tạo đã tồn tại");
            //}

            
            //if (EducationLevelRequest.IsAnnualSystem ==true && EducationLevelRequest.IsCredit == true)
            //{
            //    return ApiResponse<EducationLevelResponse>.BadRequest("Không thể áp dụng đồng thời cả hệ niên chế và hệ tín chỉ");
            //}

            existing.Name = EducationLevelRequest.Name;
            existing.TrainingType = EducationLevelRequest.TrainingType;
            existing.Status = EducationLevelRequest.Status;
            existing.Description = EducationLevelRequest.Description;
            existing.IsAnnualSystem = EducationLevelRequest.IsAnnualSystem;
            existing.IsCredit = EducationLevelRequest.IsCredit;
            existing.TrainingDuration = EducationLevelRequest.TrainingDuration;
            existing.SemesterPerYear = EducationLevelRequest.SemesterPerYear;
            existing.MandatoryCourse = EducationLevelRequest.MandatoryCourse;
            existing.ElectiveCourse = EducationLevelRequest.ElectiveCourse;

            try
            {
                var updated = _repository.UpdateEducationLevel(existing);
                return ApiResponse<EducationLevelResponse>.Success(_mapper.Map<EducationLevelResponse>(updated));
            }
            catch (Exception ex)
            {
                return ApiResponse<EducationLevelResponse>.BadRequest("Lỗi khi cập nhật cấp bậc đào tạo"); 
            }

        }

        public ApiResponse<object> DeleteEducationLevel(long id)
        {
            var success = _repository.DeleteEducationLevel(id);
            return success ? ApiResponse<object>.Success() : ApiResponse<object>.NotFound($"Không tìm thấy cập bậc đào tạo #{id} học để xóa");
        }
    }

}
