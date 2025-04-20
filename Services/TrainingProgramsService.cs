using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using System.Xml.Linq;
using System;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using CloudinaryDotNet;

namespace ISC_ELIB_SERVER.Services
{
    public class TrainingProgramsService : ITrainingProgramService
    {
        private readonly TrainingProgramsRepo _repository;
        private readonly TeacherInfoRepo _teacherInfoRepo;
        private readonly TeacherTrainingProgramRepo  _teacherTrainingProgram;
        private readonly MajorRepo _Majorrepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TrainingProgramsService(TeacherInfoRepo teacherInfoRepo, TeacherTrainingProgramRepo teacherTrainingProgram, TrainingProgramsRepo repository, MajorRepo majorrepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _teacherInfoRepo = teacherInfoRepo;
            _teacherTrainingProgram = teacherTrainingProgram;
            _Majorrepository = majorrepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public ApiResponse<ICollection<TrainingProgramsResponse>> GetTrainingPrograms(int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder)
        {
            var query = _repository.GetTrainingProgram().AsQueryable();

            query = query.Where(us => us.Active == true);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(us => us.Name.ToLower().Contains(search.ToLower()));
            }

            query = sortColumn switch
            {
                "Name" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Name) : query.OrderBy(us => us.Name),
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                _ => query.OrderBy(us => us.Id)
            };

            int totalItems = query.Count();

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            var result = query.ToList();
            var response = _mapper.Map<ICollection<TrainingProgramsResponse>>(result);

            return result.Any() ? ApiResponse<ICollection<TrainingProgramsResponse>>
            .Success(response, page, pageSize, totalItems)
            : ApiResponse<ICollection<TrainingProgramsResponse>>.NotFound("Không có dữ liệu");
        }

        //public ApiResponse<ICollection<TrainingProgramsResponse>> GetTrainingPrograms(int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder)
        //{
        //    var userID = _httpContextAccessor.HttpContext?.User?.FindFirst("Id")?.Value;

        //    if (!int.TryParse(userID, out int userId))
        //    {
        //        return ApiResponse<ICollection<TrainingProgramsResponse>>.Fail("ID trong token không hợp lệ hoặc không tồn tại.");
        //    }

        //    var teacherId = _repository.GetTeacherInfo().FirstOrDefault(t => t.UserId == userId)?.Id;

        //    if (teacherId == null)
        //    {
        //        return ApiResponse<ICollection<TrainingProgramsResponse>>.NotFound("Không tìm thấy thông tin giảng viên.");
        //    }

        //    var trainingProgramIds = _repository.GetTeacherTrainingPrograms()
        //        .Where(tt => tt.TeacherId == teacherId)
        //        .Select(tt => tt.TrainingProgramId);

        //    var query = _repository.GetTrainingProgram()
        //        .Where(tp => trainingProgramIds.Contains(tp.Id) && tp.Active == true)
        //        .AsQueryable();

        //    if (!string.IsNullOrEmpty(search))
        //    {
        //        query = query.Where(us => us.Name.ToLower().Contains(search.ToLower()));
        //    }

        //    query = sortColumn switch
        //    {
        //        "Name" => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(us => us.Name) : query.OrderBy(us => us.Name),
        //        "Id" => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
        //        _ => query.OrderBy(us => us.Id)
        //    };

        //    int totalItems = query.Count();

        //    if (page.HasValue && pageSize.HasValue)
        //    {
        //        query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
        //    }

        //    var result = query.ToList();
        //    var response = _mapper.Map<ICollection<TrainingProgramsResponse>>(result);

        //    return result.Any()
        //        ? ApiResponse<ICollection<TrainingProgramsResponse>>.Success(response, page, pageSize, totalItems)
        //        : ApiResponse<ICollection<TrainingProgramsResponse>>.NotFound("Không có dữ liệu");
        //}



        public ApiResponse<ICollection<TrainingProgramsResponse>> GetTrainingProgramsByTeacherId(long teacherId, string? search)
        {
            var trainingProgramIds = _repository.GetTeacherTrainingPrograms()
                .Where(tt => tt.TeacherId == teacherId)
                .Select(tt => tt.TrainingProgramId)
                .ToList();

            var trainingPrograms = _repository.GetTrainingProgram()
                .ToList() 
                .Where(tp => trainingProgramIds.Contains(tp.Id) && tp.Active == true)
            .ToList();

            if (!string.IsNullOrEmpty(search))
            {
                trainingPrograms = trainingPrograms
                    .Where(tp => tp.Name != null && tp.Name.ToLower().Contains(search.ToLower()))
                    .ToList();
            }

            if (!trainingPrograms.Any())
            {
                return ApiResponse<ICollection<TrainingProgramsResponse>>.NotFound("Không có chương trình đào tạo nào.");
            }

            var response = _mapper.Map<ICollection<TrainingProgramsResponse>>(trainingPrograms);
            return ApiResponse<ICollection<TrainingProgramsResponse>>.Success(response);
        }






        //public ApiResponse<TrainingProgramsResponse> GetTrainingProgramsById(long id)
        //{
        //    var trainingProgram = _repository.GetTrainingProgramById(id);
        //    return (trainingProgram != null && (trainingProgram.Active == true))
        //        ? ApiResponse<TrainingProgramsResponse>.Success(_mapper.Map<TrainingProgramsResponse>(trainingProgram))
        //        : ApiResponse<TrainingProgramsResponse>.NotFound($"Không tìm thấy chương trình đào tạo #{id}");
        //}

        public ApiResponse<TrainingProgramsResponse> GetTrainingProgramsById(long id, long teacherId)
        {
            // Kiểm tra xem chương trình có thuộc giảng viên này không
            var isAssigned = _repository.GetTeacherTrainingPrograms()
                .Any(tt => tt.TeacherId == teacherId && tt.TrainingProgramId == id);

            if (!isAssigned)
            {
                return ApiResponse<TrainingProgramsResponse>.NotFound($"Chương trình đào tạo #{id} không thuộc giảng viên này.");
            }

            var trainingProgram = _repository.GetTrainingProgramById(id);

            return (trainingProgram != null && trainingProgram.Active == true)
                ? ApiResponse<TrainingProgramsResponse>.Success(_mapper.Map<TrainingProgramsResponse>(trainingProgram))
                : ApiResponse<TrainingProgramsResponse>.NotFound($"Không tìm thấy chương trình đào tạo #{id}");
        }



        //public ApiResponse<TrainingProgramsResponse> CreateTrainingPrograms(TrainingProgramsRequest trainingProgramsRequest)
        //{
        //    if (trainingProgramsRequest.StartDate >= trainingProgramsRequest.EndDate)
        //    {
        //        return ApiResponse<TrainingProgramsResponse>.BadRequest("Ngày bắt đầu phải trước ngày kết thúc");
        //    }

        //    var existing = _repository.GetTrainingProgram().FirstOrDefault(us => us.Name?.ToLower() == trainingProgramsRequest.Name.ToLower());
        //    if (existing != null)
        //    {
        //        return ApiResponse<TrainingProgramsResponse>.Conflict("Tên chương trình đào tạo đã tồn tại");
        //    }

        //    var majorExists = _Majorrepository.GetMajor()
        //    .Any(m => m.Id == trainingProgramsRequest.MajorId);
        //    if (!majorExists)
        //    {
        //        return ApiResponse<TrainingProgramsResponse>.Conflict("Chủ đề không tồn tại");
        //    }

        //    var created = _repository.CreateTrainingProgram(new TrainingProgram()
        //    {
        //        Name = trainingProgramsRequest.Name,
        //        MajorId = trainingProgramsRequest.MajorId,
        //        SchoolFacilitiesId = trainingProgramsRequest.SchoolFacilitiesId,
        //        //StartDate = trainingProgramsRequest.StartDate,
        //        StartDate = DateTime.SpecifyKind(trainingProgramsRequest.StartDate, DateTimeKind.Utc),
        //        EndDate = DateTime.SpecifyKind(trainingProgramsRequest.EndDate, DateTimeKind.Utc),
        //        //EndDate = trainingProgramsRequest.EndDate,
        //        TrainingForm = trainingProgramsRequest.TrainingForm,
        //        Active = true,
        //        FileName = trainingProgramsRequest.FileName,
        //        FilePath = trainingProgramsRequest.FilePath,
        //        Degree = trainingProgramsRequest.Degree
        //    });
        //    return ApiResponse<TrainingProgramsResponse>.Success(_mapper.Map<TrainingProgramsResponse>(created));
        //}

        public ApiResponse<TrainingProgramsResponse> CreateTrainingPrograms(TrainingProgramsRequest trainingProgramsRequest)
        {
            // Kiểm tra ngày
            if (trainingProgramsRequest.StartDate >= trainingProgramsRequest.EndDate)
            {
                return ApiResponse<TrainingProgramsResponse>.BadRequest("Ngày bắt đầu phải trước ngày kết thúc");
            }

            // Kiểm tra tên trùng
            var existing = _repository.GetTrainingProgram().FirstOrDefault(us =>
                us.Name?.ToLower() == trainingProgramsRequest.Name.ToLower());
            if (existing != null)
            {
                return ApiResponse<TrainingProgramsResponse>.Conflict("Tên chương trình đào tạo đã tồn tại");
            }

            // Kiểm tra chuyên ngành
            var majorExists = _Majorrepository.GetMajor()
                .Any(m => m.Id == trainingProgramsRequest.MajorId);
            if (!majorExists)
            {
                return ApiResponse<TrainingProgramsResponse>.Conflict("Chủ đề không tồn tại");
            }

            // Kiểm tra teacherId truyền vào có tồn tại không
            var teacherExists = _repository.GetTeacherInfo()
                .Any(t => t.Id == trainingProgramsRequest.TeacherId);
            if (!teacherExists)
            {
                return ApiResponse<TrainingProgramsResponse>.NotFound("Không tìm thấy thông tin giảng viên.");
            }

            var created = _repository.CreateTrainingProgram(new TrainingProgram()
            {
                Name = trainingProgramsRequest.Name,
                MajorId = trainingProgramsRequest.MajorId,
                SchoolFacilitiesId = trainingProgramsRequest.SchoolFacilitiesId,
                StartDate = DateTime.SpecifyKind(trainingProgramsRequest.StartDate, DateTimeKind.Utc),
                EndDate = DateTime.SpecifyKind(trainingProgramsRequest.EndDate, DateTimeKind.Utc),
                TrainingForm = trainingProgramsRequest.TrainingForm,
                Active = true,
                FileName = trainingProgramsRequest.FileName,
                FilePath = trainingProgramsRequest.FilePath,
                Degree = trainingProgramsRequest.Degree
            });

            _teacherTrainingProgram.CreateTeacherTrainingPrograms(new TeacherTrainingProgram()
            {
                TeacherId = trainingProgramsRequest.TeacherId,
                TrainingProgramId = created.Id
            });

            return ApiResponse<TrainingProgramsResponse>.Success(_mapper.Map<TrainingProgramsResponse>(created));
        }


        //public ApiResponse<TrainingProgramsResponse> UpdateTrainingPrograms(long id, TrainingProgramsRequest trainingProgramsRequest)
        //{
        //    var existingTrainingPrograms = _repository.GetTrainingProgramById(id);
        //    if (existingTrainingPrograms == null || existingTrainingPrograms.Active == false)
        //    {
        //        return ApiResponse<TrainingProgramsResponse>.NotFound("Không tìm thấy chương trình đào tạo.");
        //    }

        //    if (trainingProgramsRequest.StartDate >= trainingProgramsRequest.EndDate)
        //    {
        //        return ApiResponse<TrainingProgramsResponse>.BadRequest("Ngày bắt đầu phải trước ngày kết thúc");
        //    }

        //    var existing = _repository.GetTrainingProgram()
        //        .FirstOrDefault(us => us.Id != id && us.Name?.ToLower() == trainingProgramsRequest.Name.ToLower());
        //    if (existing != null)
        //    {
        //        return ApiResponse<TrainingProgramsResponse>.Conflict("Tên chương trình đào tạo đã tồn tại");
        //    }

        //    existingTrainingPrograms.Name = trainingProgramsRequest.Name;
        //    existingTrainingPrograms.MajorId = trainingProgramsRequest.MajorId;
        //    existingTrainingPrograms.SchoolFacilitiesId = trainingProgramsRequest.SchoolFacilitiesId;
        //    existingTrainingPrograms.Degree = trainingProgramsRequest.Degree;
        //    //existingTrainingPrograms.StartDate = trainingProgramsRequest.StartDate;
        //    //existingTrainingPrograms.EndDate = trainingProgramsRequest.EndDate;
        //    existingTrainingPrograms.StartDate = DateTime.SpecifyKind(trainingProgramsRequest.StartDate, DateTimeKind.Utc);
        //    existingTrainingPrograms.EndDate = DateTime.SpecifyKind(trainingProgramsRequest.EndDate, DateTimeKind.Utc);
        //    existingTrainingPrograms.TrainingForm = trainingProgramsRequest.TrainingForm;
        //    //existingTrainingPrograms.Active = false;
        //    existingTrainingPrograms.FileName = trainingProgramsRequest.FileName;
        //    existingTrainingPrograms.FilePath = trainingProgramsRequest.FilePath;
        //    _repository.UpdateTrainingProgram(existingTrainingPrograms);
        //    return ApiResponse<TrainingProgramsResponse>.Success(_mapper.Map<TrainingProgramsResponse>(existingTrainingPrograms));
        //}

        public ApiResponse<TrainingProgramsResponse> UpdateTrainingPrograms(long id, TrainingProgramsRequest trainingProgramsRequest)
        {
            var isTeacherAssigned = _teacherTrainingProgram.GetTeacherTrainingPrograms()
            .Any(tt => tt.TeacherId == trainingProgramsRequest.TeacherId && tt.TrainingProgramId == id);

            if (!isTeacherAssigned)
            {
                return ApiResponse<TrainingProgramsResponse>.NotFound("Bạn không có quyền sửa chương trình đào tạo này.");
            }

            var existingTrainingPrograms = _repository.GetTrainingProgramById(id);
            if (existingTrainingPrograms == null || existingTrainingPrograms.Active == false)
            {
                return ApiResponse<TrainingProgramsResponse>.NotFound("Không tìm thấy chương trình đào tạo.");
            }

            if (trainingProgramsRequest.StartDate >= trainingProgramsRequest.EndDate)
            {
                return ApiResponse<TrainingProgramsResponse>.BadRequest("Ngày bắt đầu phải trước ngày kết thúc");
            }

            var existing = _repository.GetTrainingProgram()
                .FirstOrDefault(us => us.Id != id && us.Name?.ToLower() == trainingProgramsRequest.Name.ToLower());
            if (existing != null)
            {
                return ApiResponse<TrainingProgramsResponse>.Conflict("Tên chương trình đào tạo đã tồn tại");
            }

            existingTrainingPrograms.Name = trainingProgramsRequest.Name;
            existingTrainingPrograms.MajorId = trainingProgramsRequest.MajorId;
            existingTrainingPrograms.SchoolFacilitiesId = trainingProgramsRequest.SchoolFacilitiesId;
            existingTrainingPrograms.Degree = trainingProgramsRequest.Degree;
            existingTrainingPrograms.StartDate = DateTime.SpecifyKind(trainingProgramsRequest.StartDate, DateTimeKind.Utc);
            existingTrainingPrograms.EndDate = DateTime.SpecifyKind(trainingProgramsRequest.EndDate, DateTimeKind.Utc);
            existingTrainingPrograms.TrainingForm = trainingProgramsRequest.TrainingForm;
            existingTrainingPrograms.FileName = trainingProgramsRequest.FileName;
            existingTrainingPrograms.FilePath = trainingProgramsRequest.FilePath;

            _repository.UpdateTrainingProgram(existingTrainingPrograms);

            return ApiResponse<TrainingProgramsResponse>.Success(_mapper.Map<TrainingProgramsResponse>(existingTrainingPrograms));
        }


        //public ApiResponse<TrainingProgram> DeleteTrainingPrograms(long id)
        //{
        //    var existingTrainingProgram = _repository.GetTrainingProgramById(id);
        //    if (existingTrainingProgram == null)
        //    {
        //        return ApiResponse<TrainingProgram>.NotFound("Không tìm thấy Chương trình đào tạo.");
        //    }

        //    if (existingTrainingProgram.Active == false)
        //    {
        //        return ApiResponse<TrainingProgram>.Conflict("Chương trình đào tạo không tồn tại.");
        //    }

        //    existingTrainingProgram.Active = false;
        //    _repository.DeleteTrainingProgram(existingTrainingProgram);

        //    return ApiResponse<TrainingProgram>.Success();
        //}

        public ApiResponse<TrainingProgram> DeleteTrainingPrograms(long id, long teacherId)
        {
            var isOwned = _teacherTrainingProgram.GetTeacherTrainingPrograms()
           .Any(x => x.TeacherId == teacherId && x.TrainingProgramId == id);

            if (!isOwned)
                return ApiResponse<TrainingProgram>.NotFound("Bạn không có quyền xóa chương trình này.");


            var trainingProgram = _repository.GetTrainingProgramById(id);
            if (trainingProgram == null || trainingProgram.Active == false)
                return ApiResponse<TrainingProgram>.NotFound("Chương trình không tồn tại.");

            trainingProgram.Active = false;
            _repository.DeleteTrainingProgram(trainingProgram);

            return ApiResponse<TrainingProgram>.Success();
        }

       
    }
}
