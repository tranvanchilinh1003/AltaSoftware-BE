using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace ISC_ELIB_SERVER.Services
{

    public class AcademicYearService : IAcademicYearService
    {
        private readonly AcademicYearRepo _academicYearRepo;
        private readonly SchoolRepo _schoolRepo;
        private readonly SemesterRepo _semesterRepo;
        private readonly IMapper _mapper;

        public AcademicYearService(AcademicYearRepo academicYearRepo, SchoolRepo schoolRepo,
        SemesterRepo semesterRepo, IMapper mapper)
        {
            _academicYearRepo = academicYearRepo;
            _schoolRepo = schoolRepo;
            _semesterRepo = semesterRepo;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<AcademicYearResponse>> GetAcademicYears(
        int? page, int? pageSize, string? sortColumn, string? sortOrder)
        {
            var query = _academicYearRepo.GetAcademicYears().AsQueryable();

            query = sortColumn?.ToLower() switch
            {
                "id" when sortOrder?.ToLower() == "desc" => query.OrderByDescending(ay => ay.Id),
                "id" => query.OrderBy(ay => ay.Id),
                _ => query.OrderBy(ay => ay.Id)
            };

            int totalRecords = query.Count();

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            var result = query.ToList();
            var response = _mapper.Map<ICollection<AcademicYearResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<AcademicYearResponse>>.Success(response, page, pageSize, totalRecords)
                : ApiResponse<ICollection<AcademicYearResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<AcademicYearResponse> GetAcademicYearById(long id)
        {
            var academicYear = _academicYearRepo.GetAcademicYearById(id);
            return academicYear != null
                ? ApiResponse<AcademicYearResponse>.Success(_mapper.Map<AcademicYearResponse>(academicYear))
                : ApiResponse<AcademicYearResponse>.NotFound($"Không tìm thấy năm học #{id}");
        }
        public async Task<ApiResponse<AcademicYearResponse>> CreateAcademicYear(AcademicYearRequest academicYearRequest)
        {
            if (academicYearRequest.StartTime >= academicYearRequest.EndTime)
            {
                return ApiResponse<AcademicYearResponse>.BadRequest("Ngày bắt đầu phải trước ngày kết thúc");
            }

            var duration = (academicYearRequest.EndTime - academicYearRequest.StartTime).TotalDays / 365;
            if (duration < 1 || duration > 5)
            {
                return ApiResponse<AcademicYearResponse>.BadRequest("Niên khóa phải kéo dài ít nhất 1 năm và nhiều nhất 5 năm");
            }

            var school = _schoolRepo.GetSchoolById(1);
            if (school == null)
            {
                return ApiResponse<AcademicYearResponse>.BadRequest("Mã trường không chính xác");
            }

            var existingAcademicYears = _academicYearRepo.GetAcademicYearsBySchoolId(1);

            if (existingAcademicYears.Any(x =>
            x.StartTime == academicYearRequest.StartTime &&
            x.EndTime == academicYearRequest.EndTime))
            {
                return ApiResponse<AcademicYearResponse>.BadRequest("Niên khóa này đã tồn tại trong trường");
            }

            if (existingAcademicYears.Any(x =>
            academicYearRequest.StartTime < x.EndTime && academicYearRequest.EndTime < x.StartTime))
            {
                return ApiResponse<AcademicYearResponse>.BadRequest("Niên khóa này chồng lấn với niên khóa đã tồn tại");
            }

            var newAcademicYear = new AcademicYear
            {
                StartTime = DateTime.SpecifyKind(academicYearRequest.StartTime, DateTimeKind.Unspecified),
                EndTime = DateTime.SpecifyKind(academicYearRequest.EndTime, DateTimeKind.Unspecified),
                SchoolId = 1,
            };

            using (var transaction = await _academicYearRepo.BeginTransactionAsync())
            {
                try
                {
                    var created = _academicYearRepo.CreateAcademicYear(newAcademicYear);

                    if (academicYearRequest.Semesters != null && academicYearRequest.Semesters.Count > 0)
                    {
                        foreach (var semesterRequest in academicYearRequest.Semesters)
                        {
                            if (semesterRequest.StartTime < newAcademicYear.StartTime || semesterRequest.EndTime > newAcademicYear.EndTime)
                            {
                                transaction.Rollback();
                                return ApiResponse<AcademicYearResponse>.BadRequest("Học kỳ phải nằm trong khoảng thời gian của niên khóa");
                            }

                            var durationSemester = (semesterRequest.EndTime - semesterRequest.StartTime).TotalDays / 30;
                            if (durationSemester < 1)
                            {
                                transaction.Rollback();
                                return ApiResponse<AcademicYearResponse>.BadRequest("Thời gian của học kỳ phải kéo dài ít nhất 1 tháng");
                            }

                            var newSemester = new Semester
                            {
                                AcademicYearId = created.Id,
                                Name = semesterRequest.Name,
                                StartTime = DateTime.SpecifyKind(semesterRequest.StartTime, DateTimeKind.Unspecified),
                                EndTime = DateTime.SpecifyKind(semesterRequest.EndTime, DateTimeKind.Unspecified)
                            };
                            _semesterRepo.CreateSemester(newSemester);
                        }
                    }

                    transaction.Commit();
                    return ApiResponse<AcademicYearResponse>.Success(_mapper.Map<AcademicYearResponse>(created));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return ApiResponse<AcademicYearResponse>.BadRequest(ex.Message);
                }
            }
        }

        public ApiResponse<AcademicYearResponse> UpdateAcademicYear(long id, ICollection<AcademicYearSemesterRequest> semesterRequests)
        {
            var existing = _academicYearRepo.GetAcademicYearById(id);
            if (existing == null)
            {
                return ApiResponse<AcademicYearResponse>.NotFound($"Không tìm thấy năm học #{id}");
            }

            try
            {
                var semesters = _semesterRepo.GetSemestersByAcademicYearId(id).ToList();
                if (semesterRequests.Count > semesters.Count)
                {
                    var missingSemesters = semesterRequests.Skip(semesters.Count).ToList();
                    foreach (var newSemester in missingSemesters)
                    {
                        _semesterRepo.CreateSemester(new Semester
                        {
                            AcademicYearId = (int)id,
                            Name = newSemester.Name,
                            StartTime = newSemester.StartTime,
                            EndTime = newSemester.EndTime
                        });
                    }
                }
                else if (semesterRequests.Count == semesters.Count)
                {
                    foreach (var existingSemester in semesters)
                    {
                        var updatedSemester = semesterRequests.FirstOrDefault(s => s.Id == existingSemester.Id);
                        if (updatedSemester != null)
                        {
                            existingSemester.Name = updatedSemester.Name;
                            existingSemester.StartTime = updatedSemester.StartTime;
                            existingSemester.EndTime = updatedSemester.EndTime;
                            _semesterRepo.UpdateSemester(existingSemester);
                        }
                    }
                }
                else
                {
                    var semestersToDelete = semesters.Where(s => !semesterRequests.Any(r => r.Id == s.Id)).ToList();
                    foreach (var semester in semestersToDelete)
                    {
                        _semesterRepo.DeleteSemester(semester.Id);
                    }
                }

                return ApiResponse<AcademicYearResponse>.Success();
            }
            catch (Exception ex)
            {
                return ApiResponse<AcademicYearResponse>.BadRequest(ex.Message);
            }
        }


        public ApiResponse<object> DeleteAcademicYear(long id)
        {
            var success = _academicYearRepo.DeleteAcademicYear(id);
            return success ? ApiResponse<object>.Success() : ApiResponse<object>.NotFound($"Không tìm thấy năm #{id} học để xóa");
        }
    }

}
