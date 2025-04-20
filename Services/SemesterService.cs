using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;
using ISC_ELIB_SERVER.Services.Interfaces;

namespace ISC_ELIB_SERVER.Services
{

    public class SemesterService : ISemesterService
    {
        private readonly SemesterRepo _repository;
        private readonly IMapper _mapper;

        public SemesterService(SemesterRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<SemesterResponse>> GetSemesters(int? page, int? pageSize, string? sortColumn, string? sortOrder)
        {
            var query = _repository.GetSemesters().AsQueryable();

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

            var response = _mapper.Map<ICollection<SemesterResponse>>(result);

            return result.Any() ? ApiResponse<ICollection<SemesterResponse>>.Success(response, page, pageSize, _repository.GetSemesters().Count) : ApiResponse<ICollection<SemesterResponse>>.NotFound("Không có dữ liệu");
        }


        public ApiResponse<ICollection<object>> GetScoreBySemesters(long userId, long academicYearId)
        {
            var query = _repository.GetScoreBySemesters(userId, academicYearId).AsQueryable();

            
            var result = query.ToList();

            var response = _mapper.Map<ICollection<object>>(result);

            return result.Any() ? ApiResponse<ICollection<object>>.Success(response) : ApiResponse<ICollection<object>>.NotFound("Không có dữ liệu");
        }


        public ApiResponse<ICollection<object>> GetStudentScores(long userId, long academicYearId)
        {
            var query = _repository.GetStudentScores(userId, academicYearId).AsQueryable();


            var result = query.ToList();

            var response = _mapper.Map<ICollection<object>>(result);

            return result.Any() ? ApiResponse<ICollection<object>>.Success(response) : ApiResponse<ICollection<object>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<ICollection<object>> GetCourseOfSemesters(int? page, int? pageSize, string? sortColumn, string? sortOrder, int UserId)
        {
            var query = _repository.GetCourseOfSemesters(UserId).AsQueryable();

            //query = sortColumn switch
            //{
            //    "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
            //    _ => query.OrderBy(ay => ay.Id)
            //};


            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            var result = query.ToList();

            var response = _mapper.Map<ICollection<object>>(result);

            return result.Any() ? ApiResponse<ICollection<object>>.Success(response, page, pageSize, _repository.GetCourseOfSemesters(UserId).Count) : ApiResponse<ICollection<object>>.NotFound("Không có dữ liệu");
        }


        public ApiResponse<SemesterResponse> GetSemesterById(long id)
        {
            var Semester = _repository.GetSemesterById(id);
            return Semester != null
                ? ApiResponse<SemesterResponse>.Success(_mapper.Map<SemesterResponse>(Semester))
                : ApiResponse<SemesterResponse>.NotFound($"Không tìm thấy kỳ học #{id}");
        }

        public ApiResponse<SemesterResponse> CreateSemester(SemesterRequest SemesterRequest)
        {
            if (SemesterRequest.StartTime >= SemesterRequest.EndTime)
            {
                return ApiResponse<SemesterResponse>.BadRequest("Ngày bắt đầu phải trước ngày kết thúc");
            }

            var newSemester = new Semester
            {
                Name = SemesterRequest.Name,
                StartTime = DateTime.SpecifyKind(SemesterRequest.StartTime, DateTimeKind.Unspecified),
                EndTime = DateTime.SpecifyKind(SemesterRequest.EndTime, DateTimeKind.Unspecified),
                AcademicYearId = SemesterRequest.AcademicYearId
            };
            try
            {

                var created = _repository.CreateSemester(newSemester);
                return ApiResponse<SemesterResponse>.Success(_mapper.Map<SemesterResponse>(created));
            }
            catch (Exception ex)
            {
                return ApiResponse<SemesterResponse>.BadRequest("Mã niên khóa không chính xác");  
            }
        }

        public ApiResponse<SemesterResponse> UpdateSemester(long id, SemesterRequest SemesterRequest)
        {
            var existing = _repository.GetSemesterById(id);
            if (existing == null)
            {
                return ApiResponse<SemesterResponse>.NotFound($"Không tìm thấy kỳ học #{id}");
            }

            if (SemesterRequest.StartTime >= SemesterRequest.EndTime)
            {
                return ApiResponse<SemesterResponse>.BadRequest("Ngày bắt đầu phải trước ngày kết thúc");
            }
            existing.Name = SemesterRequest.Name;
            existing.StartTime = DateTime.SpecifyKind(SemesterRequest.StartTime, DateTimeKind.Unspecified);
            existing.EndTime = DateTime.SpecifyKind(SemesterRequest.EndTime, DateTimeKind.Unspecified);
            existing.AcademicYearId = SemesterRequest.AcademicYearId;
            try
            {


                var updated = _repository.UpdateSemester(existing);
                return ApiResponse<SemesterResponse>.Success(_mapper.Map<SemesterResponse>(updated));
            }
            catch (Exception ex)
            {
                return ApiResponse<SemesterResponse>.BadRequest("Mã niên khóa không chính xác");
            }

        }

        public ApiResponse<object> DeleteSemester(long id)
        {
            var success = _repository.DeleteSemester(id);
            return success ? ApiResponse<object>.Success() : ApiResponse<object>.NotFound($"Không tìm thấy kỳ học #{id} để xóa");
        }
    }

}
