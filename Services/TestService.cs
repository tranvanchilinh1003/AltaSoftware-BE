using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;
using Sprache;
using System.Xml.Linq;
using System.Security.Claims;
using System;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Services
{
       
    public class TestService : ITestService
    {
            private readonly GradeLevelRepo _gradeLevelRepo;
            private readonly TestRepo _testRepo;
            private readonly SemesterRepo _semesterRepo;
            private readonly SubjectRepo _subjectRepo;
            private readonly UserRepo _userRepo;
            private readonly SubjectGroupRepo _subjectGroupRepo;
            private readonly isc_dbContext _context; 
            private readonly CloudinaryService _cloudinaryService;
            private readonly IMapper _mapper;

            public TestService(TestRepo testRepo, IMapper mapper, SemesterRepo semesterRepo, UserRepo userRepo, SubjectRepo subjectRepo, GradeLevelRepo gradeLevelRepo, SubjectGroupRepo subjectGroupRepo, isc_dbContext context, CloudinaryService cloudinaryService)
            {
                _testRepo = testRepo;
                _userRepo = userRepo;
                _semesterRepo = semesterRepo;
                _subjectRepo = subjectRepo;
                _mapper = mapper;
                _gradeLevelRepo = gradeLevelRepo;
                _subjectGroupRepo = subjectGroupRepo;
                _context = context;
            _cloudinaryService = cloudinaryService;
            }

            public ApiResponse<ICollection<TestResponse>> GetTestes(int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder)
            {
                var query = _testRepo.GetTests().AsQueryable();

                query = query.Where(qr => qr.Active.HasValue && qr.Active.Value);

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(ts => ts.Name.ToLower().Contains(search.ToLower()));
                }

                query = sortColumn?.ToLower() switch
                {
                    "name" => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(ts => ts.Name) : query.OrderBy(ts => ts.Name),
                    "id" => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(ts => ts.Id) : query.OrderBy(ts => ts.Id),
                    _ => query.OrderBy(us => us.Id)
                };
                query = query.Where(qr => qr.Active == true);

                var total = query.Count();

            
                if (page.HasValue && pageSize.HasValue)
                {
                    query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }
                var result = query.ToList();

                var response = _mapper.Map<ICollection<TestResponse>>(result);

                return result.Any()
                        ? ApiResponse<ICollection<TestResponse>>.Success(
                                data: response,
                                totalItems: total,
                                pageSize: pageSize,
                                page: page
                            )
                        : ApiResponse<ICollection<TestResponse>>.NotFound("Không có dữ liệu");
            }

        public ApiResponse<ICollection<TestByStudentResponse>> GetTestesByStudent(int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder, int status, long? subjectGroupId, long? gradeLevelsId, string? date, string? idUser)
        {
            if (string.IsNullOrEmpty(idUser))
            {
                return ApiResponse<ICollection<TestByStudentResponse>>.Fail("Không tìm thấy ID trong token");
            }

            if (!int.TryParse(idUser, out int userId))
            {
                return ApiResponse<ICollection<TestByStudentResponse>>.Fail("ID trong token không hợp lệ");
            }

            var query = _testRepo.GetTestsByStudent(userId).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(ts => ts.Test.Name.ToLower().Contains(search.ToLower()));
            }

            query = sortColumn?.ToLower() switch
            {
                "name" => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(ts => ts.Test.Name) : query.OrderBy(ts => ts.Test.Name),
                "id" => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(ts => ts.Test.Id) : query.OrderBy(ts => ts.Test.Id),
                _ => query.OrderBy(us => us.Test.Id)
            };

            if (status == 1) // Trạng thái sắp tới
            {
                var now = DateTime.Now;
                query = query.Where(qr => qr.Test.EndTime > now);
            }
            else if (status == 2)
            { // Trạng thái đã hoàn thành
                var now = DateTime.Now;
                query = query.Where(qr => qr.Test.EndTime <= now);
            }

            if (subjectGroupId != null || subjectGroupId.HasValue)
            {
                var subjectGroup = _subjectGroupRepo.GetSubjectGroupById(subjectGroupId.Value);
                if (subjectGroup == null)
                {
                    return ApiResponse<ICollection<TestByStudentResponse>>.NotFound($"Môn học có {subjectGroupId} không tồn tại!!!");
                }
                else
                {
                    query = query.Where(qr => qr.Test.Subject.SubjectGroup.Id == subjectGroupId.Value);
                }
            }

            if (gradeLevelsId != null || gradeLevelsId.HasValue)
            {
                var gradeLevel = _subjectRepo.GetSubjectById(gradeLevelsId.Value);
                if (gradeLevel == null)
                {
                    return ApiResponse<ICollection<TestByStudentResponse>>.NotFound($"Khoa khối có {gradeLevelsId} không tồn tại!!!");
                }
                else
                {
                    query = query.Where(qr => qr.Test.GradeLevel.Id == gradeLevelsId.Value);
                }
            }

            if (date != null)
            {
                DateTime dateValue;
                var check = DateTime.TryParse(date, out dateValue);
                if (check)
                {
                    query = query.Where(qr => qr.Test.StartTime.HasValue && qr.Test.StartTime.Value.Date == dateValue.Date);
                }
                else
                {
                    return ApiResponse<ICollection<TestByStudentResponse>>.BadRequest($"Date không đúng định dạng!!!");
                }
            }

            var total = query.Count();

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            var result = query.ToList();

            var response = _mapper.Map<ICollection<TestByStudentResponse>>(result);

            return result.Any()
                    ? ApiResponse<ICollection<TestByStudentResponse>>.Success(
                            data: response,
                            totalItems: total,
                            pageSize: pageSize,
                            page: page
                        )
                    : ApiResponse<ICollection<TestByStudentResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<TestResponse> GetTestById(long id)
            {
                var Test = _testRepo.GetTestById(id);
                return Test != null
                    ? ApiResponse<TestResponse>.Success(_mapper.Map<TestResponse>(Test))
                    : ApiResponse<TestResponse>.NotFound($"Không tìm thấy bài kiểm tra #{id}");
            }

            public ApiResponse<TestResponse> GetTestByName(string name)
            {
                

                var Test = _testRepo.GetTests().ToList().FirstOrDefault(ts => ts.Name?.ToLower() == name.ToLower());
                return Test != null
                    ? ApiResponse<TestResponse>.Success(_mapper.Map<TestResponse>(Test))
                    : ApiResponse<TestResponse>.NotFound($"Không tìm thấy bài kiểm tra có tên: {name}");
            }

        public ApiResponse<TestResponse> CreateTest(TestRequest testRequest, string? idUser)
        {
            if (string.IsNullOrEmpty(idUser))
            {
                return ApiResponse<TestResponse>.Fail("Không tìm thấy ID trong token");
            }

            if (!int.TryParse(idUser, out int userId))
            {
                return ApiResponse<TestResponse>.Fail("ID trong token không hợp lệ");
            }

            var grade_levels = _gradeLevelRepo.GetGradeLevelById(testRequest.GradeLevelsId);
            if(grade_levels == null)
            {
                return ApiResponse<TestResponse>.NotFound($"Học kỳ có id {testRequest.GradeLevelsId} không tồn tạ!!!");
            }
            var subject = _subjectRepo.GetSubjectById(testRequest.SubjectId);
            if (subject == null)
            {
                return ApiResponse<TestResponse>.NotFound($"Môn học có id {testRequest.SubjectId} không tồn tạ!!!");
            }

            //var user = _userRepo.GetUserById(userId);
            //if (user == null)
            //{
            //    return ApiResponse<TestResponse>.NotFound($"Người dùng có id {userId} không tồn tạ!!!");
            //}

            var teacher = _userRepo.GetUserById(testRequest.TeacherId.Value);
            if (teacher == null)
            {
                return ApiResponse<TestResponse>.NotFound($"Người dùng có id {teacher.Id} không tồn tạ!!!");
            }

            //var cloudinaryUrl = _cloudinaryService.UploadBase64Async(testRequest.File).Result;
            //testRequest.File = cloudinaryUrl;
            var testEntity = _mapper.Map<Test>(testRequest);
            testEntity.UserId = teacher.Id;
            testEntity.StartTime = testEntity.StartTime.HasValue?DateTime.SpecifyKind(testEntity.StartTime.Value, DateTimeKind.Unspecified):null;
            testEntity.EndTime = testEntity.EndTime.HasValue ? DateTime.SpecifyKind(testEntity.EndTime.Value, DateTimeKind.Unspecified) : null;
            // Tạo mới bài kiểm tra
            var created = _testRepo.CreateTest(testEntity);

            // Trả về kết quả với kiểu TestResponse
            return ApiResponse<TestResponse>.Success(_mapper.Map<TestResponse>(created));
         }

    public ApiResponse<TestResponse> UpdateTest(long id,TestRequest testRequest, string? idUser)
    {
            if (string.IsNullOrEmpty(idUser))
            {
                return ApiResponse<TestResponse>.Fail("Không tìm thấy ID trong token");
            }

            if (!int.TryParse(idUser, out int userId))
            {
                return ApiResponse<TestResponse>.Fail("ID trong token không hợp lệ");
            }


            // Tìm bản ghi cần cập nhật trong database
            var existingTest = _testRepo.GetTestById(id);
                if (existingTest == null)
                {
                    return ApiResponse<TestResponse>.NotFound($"Bài kiểm tra có id {id} không tồn tạ!!!");
                }

            var gradeLevelsId = _gradeLevelRepo.GetGradeLevelById(testRequest.GradeLevelsId);
            if (gradeLevelsId == null)
            {
                return ApiResponse<TestResponse>.NotFound($"Học kỳ có id {testRequest.GradeLevelsId} không tồn tạ!!!");
            }
            var subject = _subjectRepo.GetSubjectById(testRequest.SubjectId);
            if (subject == null)
            {
                return ApiResponse<TestResponse>.NotFound($"Môn học có id {testRequest.SubjectId} không tồn tạ!!!");
            }

            //var user = _userRepo.GetUserById(userId);
            //if (user == null)
            //{
            //    return ApiResponse<TestResponse>.NotFound($"Người dùng có id {userId} không tồn tạ!!!");
            //}

            var teacher = _userRepo.GetUserById(testRequest.TeacherId.Value);
            if (teacher == null)
            {
                return ApiResponse<TestResponse>.NotFound($"Người dùng có id {teacher.Id} không tồn tạ!!!");
            }
            //var cloudinaryUrl = _cloudinaryService.UploadBase64Async(testRequest.File).Result;
            //testRequest.File = cloudinaryUrl;
            // Ánh xạ dữ liệu từ request sang entity, chỉ cập nhật các trường cần thiết
            _mapper.Map(testRequest, existingTest);
            existingTest.UserId = teacher.Id;
            existingTest.StartTime = existingTest.StartTime.HasValue ? DateTime.SpecifyKind(existingTest.StartTime.Value, DateTimeKind.Unspecified) : null;
            existingTest.EndTime = existingTest.EndTime.HasValue ? DateTime.SpecifyKind(existingTest.EndTime.Value, DateTimeKind.Unspecified) : null;

            // Thực hiện cập nhật bản ghi
            var updated = _testRepo.UpdateTest(existingTest);
                return ApiResponse<TestResponse>.Success(_mapper.Map<TestResponse>(updated));
     }

    public ApiResponse<Test> DeleteTest(long id)
         {
            var success = _testRepo.DeleteTest(id);
            return success
                    ? ApiResponse<Test>.Success()
                    : ApiResponse<Test>.NotFound("Không tìm thấy bài kiểm tra để xóa");
         }
    }
}

