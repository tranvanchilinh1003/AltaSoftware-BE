using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Enums;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Services
{
    public class AchivementService : IAchivementService
    {
        private readonly AchivementRepo _repository;
        private readonly IMapper _mapper;

        public AchivementService(AchivementRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<AchivementResponse>> GetAchivements(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetAchievements()
                .Include(a => a.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(a => a.Content.Contains(search));
            }

            query = sortColumn switch
            {
                "Content" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Content) : query.OrderBy(us => us.Content),
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                _ => query.OrderBy(us => us.Id)
            };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var response = result.Select(achievement => new AchivementResponse
            {
                Id = achievement.Id,
                Content = achievement.Content,
                DateAwarded = achievement.DateAwarded,
                TypeId = achievement.TypeId,
                File = achievement.File,
                Users = achievement.User != null ? new UserResponse
                {
                    Id = achievement.User.Id,
                    Code = achievement.User.Code,
                    FullName = achievement.User.FullName ?? "",
                    Gender = achievement.User.Gender,
                    Email = string.IsNullOrEmpty(achievement.User.Email) ? null : achievement.User.Email,
                    PhoneNumber = string.IsNullOrEmpty(achievement.User.PhoneNumber) ? null : achievement.User.PhoneNumber,
                    PlaceBirth = string.IsNullOrEmpty(achievement.User.PlaceBirth) ? null : achievement.User.PlaceBirth,
                    Nation = string.IsNullOrEmpty(achievement.User.Nation) ? null : achievement.User.Nation,
                    Religion = string.IsNullOrEmpty(achievement.User.Religion) ? null : achievement.User.Religion,
                    AddressFull = string.IsNullOrEmpty(achievement.User.AddressFull) ? null : achievement.User.AddressFull,
                    Street = string.IsNullOrEmpty(achievement.User.Street) ? null : achievement.User.Street,
                    Dob = achievement.User.Dob != DateTime.MinValue ? achievement.User.Dob : null,
                    EnrollmentDate = achievement.User.EnrollmentDate != DateTime.MinValue ? achievement.User.EnrollmentDate : null,
                    RoleId = achievement.User.Role?.Id ?? 0,
                    AcademicYearId = achievement.User.AcademicYear?.Id ?? 0,
                    ClassId = achievement.User.Class?.Id ?? 0,
                } : null,
                TypeValue = achievement.TypeId.HasValue && Enum.IsDefined(typeof(EType), achievement.TypeId.Value)
                            ? ((EType)achievement.TypeId.Value).ToString()
                            : "Không xác định"
            }).ToList();



            return response.Any()
                ? ApiResponse<ICollection<AchivementResponse>>.Success(response)
                : ApiResponse<ICollection<AchivementResponse>>.NotFound("Không có dữ liệu");
        }


        public ApiResponse<AchivementResponse> GetAchivementById(int id)
        {

            var achievement = _repository.GetAchivementById(id);

            if (achievement == null)
            {
                return ApiResponse<AchivementResponse>.NotFound("Không tìm thấy");
            }

            var response = new AchivementResponse
            {
                Id = achievement.Id,
                Content = achievement.Content,
                DateAwarded = achievement.DateAwarded,
                TypeId = achievement.TypeId,
                File = achievement.File,
                Users = achievement.User != null ? new UserResponse
                {
                    Id = achievement.User.Id,
                    Code = achievement.User.Code,
                    FullName = achievement.User.FullName ?? "",
                    Gender = achievement.User.Gender,
                    Email = string.IsNullOrEmpty(achievement.User.Email) ? null : achievement.User.Email,
                    PhoneNumber = string.IsNullOrEmpty(achievement.User.PhoneNumber) ? null : achievement.User.PhoneNumber,
                    PlaceBirth = string.IsNullOrEmpty(achievement.User.PlaceBirth) ? null : achievement.User.PlaceBirth,
                    Nation = string.IsNullOrEmpty(achievement.User.Nation) ? null : achievement.User.Nation,
                    Religion = string.IsNullOrEmpty(achievement.User.Religion) ? null : achievement.User.Religion,
                    AddressFull = string.IsNullOrEmpty(achievement.User.AddressFull) ? null : achievement.User.AddressFull,
                    Street = string.IsNullOrEmpty(achievement.User.Street) ? null : achievement.User.Street,
                    Dob = achievement.User.Dob != DateTime.MinValue ? achievement.User.Dob : null,
                    EnrollmentDate = achievement.User.EnrollmentDate != DateTime.MinValue ? achievement.User.EnrollmentDate : null,
                    RoleId = achievement.User.Role?.Id ?? 0,
                    AcademicYearId = achievement.User.AcademicYear?.Id ?? 0,
                    ClassId = achievement.User.Class?.Id ?? 0,
                } : null,
                TypeValue = achievement.TypeId.HasValue && Enum.IsDefined(typeof(EType), achievement.TypeId.Value)
                            ? ((EType)achievement.TypeId.Value).ToString()
                            : "Không xác định"
            };

            return ApiResponse<AchivementResponse>.Success(response);
        }

        public ApiResponse<AchivementResponse> CreateAchivement(AchivementRequest achivementRequest)
        {

            try
            {

                if (achivementRequest is null)
                {
                    return ApiResponse<AchivementResponse>.BadRequest("Dữ liệu đầu vào không hợp lệ");
                }

                var achivement = new Achievement
                {
                    UserId = achivementRequest.UserId,
                    DateAwarded = DateTime.SpecifyKind(achivementRequest.DateAwarded, DateTimeKind.Unspecified),
                    Content = achivementRequest.Content,
                    TypeId = achivementRequest?.TypeId,
                    File = achivementRequest?.File,
                };

                var created = _repository.CreateAchivement(achivement);

                var response = new AchivementResponse
                {
                    Id = created.Id,
                    Content = created.Content,
                    DateAwarded = created.DateAwarded,
                    TypeId = created.TypeId,
                    File = created.File,
                    Users = created.User != null ? new UserResponse
                    {
                        Id = created.User.Id,
                        Code = created.User.Code,
                        FullName = created.User.FullName ?? "",
                        Gender = created.User.Gender,
                        Email = string.IsNullOrEmpty(created.User.Email) ? null : created.User.Email,
                        PhoneNumber = string.IsNullOrEmpty(created.User.PhoneNumber) ? null : created.User.PhoneNumber,
                        PlaceBirth = string.IsNullOrEmpty(created.User.PlaceBirth) ? null : created.User.PlaceBirth,
                        Nation = string.IsNullOrEmpty(created.User.Nation) ? null : created.User.Nation,
                        Religion = string.IsNullOrEmpty(created.User.Religion) ? null : created.User.Religion,
                        AddressFull = string.IsNullOrEmpty(created.User.AddressFull) ? null : created.User.AddressFull,
                        Street = string.IsNullOrEmpty(created.User.Street) ? null : created.User.Street,
                        Dob = created.User.Dob != DateTime.MinValue ? created.User.Dob : null,
                        EnrollmentDate = created.User.EnrollmentDate != DateTime.MinValue ? created.User.EnrollmentDate : null,
                        RoleId = created.User.Role?.Id ?? 0,
                        AcademicYearId = created.User.AcademicYear?.Id ?? 0,
                        ClassId = created.User.Class?.Id ?? 0,
                    } : null,
                    TypeValue = created.TypeId.HasValue && Enum.IsDefined(typeof(EType), created.TypeId.Value)
                                ? ((EType)created.TypeId.Value).ToString()
                                : "Không xác định"
                };

                return ApiResponse<AchivementResponse>.Success(_mapper.Map<AchivementResponse>(response));
            }
            catch (Exception)
            {
                return ApiResponse<AchivementResponse>.BadRequest("Dữ liệu đầu vào không hợp lệ");
            }
        }

        public ApiResponse<AchivementResponse> UpdateAchivement(int id, AchivementRequest achivementRequest)
        {
            try
            {
                var achievement = _repository.GetAchivementById(id);
                if (achievement == null)
                {
                    return ApiResponse<AchivementResponse>.NotFound($"Không tìm thấy loại đầu vào #{id}");
                }

                achievement.UserId = achivementRequest.UserId;
                achievement.TypeId = achivementRequest.TypeId;
                achievement.Content = achivementRequest.Content;
                achievement.File = achivementRequest.File;
                achievement.DateAwarded = DateTime.SpecifyKind(achivementRequest.DateAwarded, DateTimeKind.Unspecified);

                var updated = _repository.UpdateAchivement(achievement);

                if (updated == null)
                {
                    return ApiResponse<AchivementResponse>.BadRequest("Cập nhật thất bại");
                }

                var response = new AchivementResponse
                {
                    Id = updated.Id,
                    Content = updated.Content,
                    DateAwarded = updated.DateAwarded,
                    TypeId = updated.TypeId,
                    File = updated.File,
                    Users = updated.User != null ? new UserResponse
                    {
                        Id = updated.User.Id,
                        Code = updated.User.Code,
                        FullName = updated.User.FullName ?? "",
                        Gender = updated.User.Gender,
                        Email = string.IsNullOrEmpty(updated.User.Email) ? null : updated.User.Email,
                        PhoneNumber = string.IsNullOrEmpty(updated.User.PhoneNumber) ? null : updated.User.PhoneNumber,
                        PlaceBirth = string.IsNullOrEmpty(updated.User.PlaceBirth) ? null : updated.User.PlaceBirth,
                        Nation = string.IsNullOrEmpty(updated.User.Nation) ? null : updated.User.Nation,
                        Religion = string.IsNullOrEmpty(updated.User.Religion) ? null : updated.User.Religion,
                        AddressFull = string.IsNullOrEmpty(updated.User.AddressFull) ? null : updated.User.AddressFull,
                        Street = string.IsNullOrEmpty(updated.User.Street) ? null : updated.User.Street,
                        Dob = updated.User.Dob != DateTime.MinValue ? updated.User.Dob : null,
                        EnrollmentDate = updated.User.EnrollmentDate != DateTime.MinValue ? updated.User.EnrollmentDate : null,
                        RoleId = updated.User.Role?.Id ?? 0,
                        AcademicYearId = updated.User.AcademicYear?.Id ?? 0,
                        ClassId = updated.User.Class?.Id ?? 0,
                    } : null,
                    TypeValue = updated.TypeId.HasValue && Enum.IsDefined(typeof(EType), updated.TypeId.Value)
                                ? ((EType)updated.TypeId.Value).ToString()
                                : "Không xác định"
                };

                return updated != null
                    ? ApiResponse<AchivementResponse>.Success(response)
                    : ApiResponse<AchivementResponse>.NotFound("Không tìm thấy vai trò để sửa");
            }
            catch
            {
                return ApiResponse<AchivementResponse>.BadRequest("Cập nhật thất bại");
            }
        }

        public ApiResponse<bool> DeleteAchivement(int id)
        {
            var success = _repository.DeleteAchivement(id);
            return success
                ? ApiResponse<bool>.Success(true)
                : ApiResponse<bool>.NotFound("Không tìm thấy vai trò để xóa");
        }

        public ApiResponse<ICollection<AchivementResponse>> GetAwards(int page, int pageSize, string search, string sortColumn, string sortOrder, int typeId = 0)
        {
            try
            {
                var query = _repository.GetAchievementsByTypeId(typeId)
                .Include(a => a.User)
                .AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(a => a.Content.Contains(search));
                }

                query = sortColumn switch
                {
                    "Content" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Content) : query.OrderBy(us => us.Content),
                    "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                    _ => query.OrderBy(us => us.Id)
                };

                var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var response = result.Select(achievement => new AchivementResponse
                {
                    Id = achievement.Id,
                    Content = achievement.Content,
                    DateAwarded = achievement.DateAwarded,
                    TypeId = achievement.TypeId,
                    File = achievement.File,
                    Users = achievement.User != null ? new UserResponse
                    {
                        Id = achievement.User.Id,
                        Code = achievement.User.Code,
                        FullName = achievement.User.FullName ?? "",
                        Gender = achievement.User.Gender,
                        Email = string.IsNullOrEmpty(achievement.User.Email) ? null : achievement.User.Email,
                        PhoneNumber = string.IsNullOrEmpty(achievement.User.PhoneNumber) ? null : achievement.User.PhoneNumber,
                        PlaceBirth = string.IsNullOrEmpty(achievement.User.PlaceBirth) ? null : achievement.User.PlaceBirth,
                        Nation = string.IsNullOrEmpty(achievement.User.Nation) ? null : achievement.User.Nation,
                        Religion = string.IsNullOrEmpty(achievement.User.Religion) ? null : achievement.User.Religion,
                        AddressFull = string.IsNullOrEmpty(achievement.User.AddressFull) ? null : achievement.User.AddressFull,
                        Street = string.IsNullOrEmpty(achievement.User.Street) ? null : achievement.User.Street,
                        Dob = achievement.User.Dob != DateTime.MinValue ? achievement.User.Dob : null,
                        EnrollmentDate = achievement.User.EnrollmentDate != DateTime.MinValue ? achievement.User.EnrollmentDate : null,
                        RoleId = achievement.User.Role?.Id ?? 0,
                        AcademicYearId = achievement.User.AcademicYear?.Id ?? 0,
                        ClassId = achievement.User.Class?.Id ?? 0,
                    } : null,
                    TypeValue = achievement.TypeId.HasValue && Enum.IsDefined(typeof(EType), achievement.TypeId.Value)
                                ? ((EType)achievement.TypeId.Value).ToString()
                                : "Không xác định"
                }).ToList();



                return response.Any()
                    ? ApiResponse<ICollection<AchivementResponse>>.Success(response)
                    : ApiResponse<ICollection<AchivementResponse>>.NotFound("Không có dữ liệu");
            }
            catch
            {
                return ApiResponse<ICollection<AchivementResponse>>.Fail("Lỗi khi lấy dữ liệu");
            }
            
        }

        public ApiResponse<ICollection<AchivementResponse>> GetDisciplines(int page, int pageSize, string search, string sortColumn, string sortOrder, int typeId = 1)
        {
            try
            {
                var query = _repository.GetAchievementsByTypeId(typeId)
                .Include(a => a.User)
                .AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(a => a.Content.Contains(search));
                }

                query = sortColumn switch
                {
                    "Content" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Content) : query.OrderBy(us => us.Content),
                    "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                    _ => query.OrderBy(us => us.Id)
                };

                var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var response = result.Select(achievement => new AchivementResponse
                {
                    Id = achievement.Id,
                    Content = achievement.Content,
                    DateAwarded = achievement.DateAwarded,
                    TypeId = achievement.TypeId,
                    File = achievement.File,
                    Users = achievement.User != null ? new UserResponse
                    {
                        Id = achievement.User.Id,
                        Code = achievement.User.Code,
                        FullName = achievement.User.FullName ?? "",
                        Gender = achievement.User.Gender,
                        Email = string.IsNullOrEmpty(achievement.User.Email) ? null : achievement.User.Email,
                        PhoneNumber = string.IsNullOrEmpty(achievement.User.PhoneNumber) ? null : achievement.User.PhoneNumber,
                        PlaceBirth = string.IsNullOrEmpty(achievement.User.PlaceBirth) ? null : achievement.User.PlaceBirth,
                        Nation = string.IsNullOrEmpty(achievement.User.Nation) ? null : achievement.User.Nation,
                        Religion = string.IsNullOrEmpty(achievement.User.Religion) ? null : achievement.User.Religion,
                        AddressFull = string.IsNullOrEmpty(achievement.User.AddressFull) ? null : achievement.User.AddressFull,
                        Street = string.IsNullOrEmpty(achievement.User.Street) ? null : achievement.User.Street,
                        Dob = achievement.User.Dob != DateTime.MinValue ? achievement.User.Dob : null,
                        EnrollmentDate = achievement.User.EnrollmentDate != DateTime.MinValue ? achievement.User.EnrollmentDate : null,
                        RoleId = achievement.User.Role?.Id ?? 0,
                        AcademicYearId = achievement.User.AcademicYear?.Id ?? 0,
                        ClassId = achievement.User.Class?.Id ?? 0,
                    } : null,
                    TypeValue = achievement.TypeId.HasValue && Enum.IsDefined(typeof(EType), achievement.TypeId.Value)
                                ? ((EType)achievement.TypeId.Value).ToString()
                                : "Không xác định"
                }).ToList();



                return response.Any()
                    ? ApiResponse<ICollection<AchivementResponse>>.Success(response)
                    : ApiResponse<ICollection<AchivementResponse>>.NotFound("Không có dữ liệu");
            }
            catch
            {
                return ApiResponse<ICollection<AchivementResponse>>.Fail("Lỗi khi lấy dữ liệu");
            }
            
        }
    }
}
