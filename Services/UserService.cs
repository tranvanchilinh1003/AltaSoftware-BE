using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace ISC_ELIB_SERVER.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepo _userRepo;
        private readonly ClassSubjectRepo _classSubjectRepo;
        private readonly RoleRepo _roleRepo;
        private readonly AcademicYearRepo _academicYearRepo;
        private readonly UserStatusRepo _userStatusRepo;
        private readonly CloudinaryService _cloudinaryService;
        private readonly ClassRepo _classRepo;
        private readonly IMapper _mapper;
        private readonly GhnService _ghnService;

        public UserService(UserRepo userRepo, RoleRepo roleRepo, AcademicYearRepo academicYearRepo, CloudinaryService cloudinaryService,
            UserStatusRepo userStatusRepo, ClassRepo classRepo, IMapper mapper, GhnService ghnService,
            ClassSubjectRepo classSubjectRepo)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _academicYearRepo = academicYearRepo;
            _cloudinaryService = cloudinaryService;
            _userStatusRepo = userStatusRepo;
            _classRepo = classRepo;
            _mapper = mapper;
            _ghnService = ghnService;
            _classSubjectRepo = classSubjectRepo;
        }

        public async Task<ApiResponse<ICollection<UserResponse>>> GetUsers(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _userRepo.GetUsers().AsQueryable();

            // Tìm kiếm người dùng theo tên, email, hoặc code
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u => u.FullName.ToLower().Contains(search.ToLower()) ||
                                          u.Email.ToLower().Contains(search.ToLower()) ||
                                          u.Code.ToLower().Contains(search.ToLower()));
            }

            // Sắp xếp theo các cột được chỉ định
            query = sortColumn switch
            {
                "FullName" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(u => u.FullName) : query.OrderBy(u => u.FullName),
                "Email" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(u => u.Id) : query.OrderBy(u => u.Id),
                _ => query.OrderBy(u => u.Id)
            };

            var result = query.ToList();
            var responses = new List<UserResponse>();

            // Lấy địa chỉ đầy đủ từ GHN API cho từng user
            foreach (var user in result)
            {
                var (provinceName, districtName, wardName) = await _ghnService.GetLocationName(user.ProvinceCode ?? 0, user.DistrictCode ?? 0, user.WardCode?.ToString() ?? "");
                var response = _mapper.Map<UserResponse>(user);
                //response.ProvinceName = provinceName;
                //response.DistrictName = districtName;
                //response.WardName = wardName;
                // RoleName bằng cách lấy từ RoleRepo
                var role = _roleRepo.GetRoleById(user.RoleId ?? 0);
                response.RoleName = role?.Name;
                // Lấy thông tin khối từ ClassRepo
                var classInfo = _classRepo.GetClassById(user.ClassId ?? 0);
                response.ClassId = classInfo != null ? user.ClassId : null;
                response.GradeLevelId = classInfo?.GradeLevelId;
                responses.Add(response);
            }

            return responses.Any() ? ApiResponse<ICollection<UserResponse>>
                .Success(responses, page, pageSize, _userRepo.GetUsers().Count)
                : ApiResponse<ICollection<UserResponse>>.NotFound("Không có dữ liệu");
        }

        public async Task<ApiResponse<UserResponse>> GetUserById(int id)
        {
            var user = _userRepo.GetUserById(id);
            if (user == null) return ApiResponse<UserResponse>.NotFound($"Không tìm thấy người dùng với ID #{id}");

            var (provinceName, districtName, wardName) = await _ghnService.GetLocationName(user.ProvinceCode ?? 0, user.DistrictCode ?? 0, user.WardCode?.ToString() ?? "");
            var response = _mapper.Map<UserResponse>(user);
            //response.ProvinceName = provinceName;
            //response.DistrictName = districtName;
            //response.WardName = wardName;
            // RoleName bằng cách lấy từ RoleRepo
            var role = _roleRepo.GetRoleById(user.RoleId ?? 0);
            response.RoleName = role?.Name;
            // Lấy thông tin khối từ ClassRepo
            var classInfo = _classRepo.GetClassById(user.ClassId ?? 0);
            response.ClassId = classInfo != null ? user.ClassId : null;
            response.GradeLevelId = classInfo?.GradeLevelId;

            return ApiResponse<UserResponse>.Success(response);
        }

        public async Task<ApiResponse<UserResponse>> GetUserByCode(string code)
        {
            var user = _userRepo.GetUsers().FirstOrDefault(u => u.Code?.ToLower() == code.ToLower());
            if (user == null) return ApiResponse<UserResponse>.NotFound($"Không tìm thấy người dùng với mã {code}");

            var (provinceName, districtName, wardName) = await _ghnService.GetLocationName(user.ProvinceCode ?? 0, user.DistrictCode ?? 0, user.WardCode?.ToString() ?? "");
            var response = _mapper.Map<UserResponse>(user);
            //response.ProvinceName = provinceName;
            //response.DistrictName = districtName;
            //response.WardName = wardName;
            // RoleName bằng cách lấy từ RoleRepo
            var role = _roleRepo.GetRoleById(user.RoleId ?? 0);
            response.RoleName = role?.Name;
            return ApiResponse<UserResponse>.Success(response);
        }

        public ApiResponse<int> GetQuantityUserByRoleId(int roleId)
        {

            try
            {
                if (_roleRepo.GetRoleById(roleId) == null)
                {
                    return ApiResponse<int>.BadRequest("RoleId không hợp lệ");
                }

                return ApiResponse<int>.Success(_userRepo.GetQuantityUserByRoleId(roleId));
            }
            catch
            {
                return ApiResponse<int>.Fail("Lỗi không thể lấy số lượng users");
            }

        }

        public ApiResponse<UserResponse> CreateUser(UserRequest userRequest)
        {
            // Kiểm tra RoleId có tồn tại không
            if (_roleRepo.GetRoleById(userRequest.RoleId) == null)
            {
                return ApiResponse<UserResponse>.BadRequest("RoleId không hợp lệ");
            }

            // Kiểm tra AcademicYearId có tồn tại không (nếu có nhập)
            if (userRequest.AcademicYearId.HasValue &&
                _academicYearRepo.GetAcademicYearById(userRequest.AcademicYearId.Value) == null)
            {
                return ApiResponse<UserResponse>.BadRequest("AcademicYearId không hợp lệ");
            }

            // Kiểm tra UserStatusId có tồn tại không (nếu có nhập)
            if (userRequest.UserStatusId.HasValue &&
                _userStatusRepo.GetUserStatusById(userRequest.UserStatusId.Value) == null)
            {
                return ApiResponse<UserResponse>.BadRequest("UserStatusId không hợp lệ");
            }

            // Kiểm tra ClassId có tồn tại không (nếu có nhập)
            if (userRequest.ClassId.HasValue &&
                _classRepo.GetClassById(userRequest.ClassId.Value) == null)
            {
                return ApiResponse<UserResponse>.BadRequest("ClassId không hợp lệ");
            }

            // Kiểm tra mã người dùng đã tồn tại chưa
            if (_userRepo.GetUserByCode(userRequest.Code) != null)
            {
                return ApiResponse<UserResponse>.BadRequest("Mã người dùng đã tồn tại");
            }

            // Kiểm tra email đã tồn tại chưa
            if (_userRepo.GetUsers().Any(u => u.Email == userRequest.Email))
            {
                return ApiResponse<UserResponse>.BadRequest("Email đã tồn tại");
            }
            // Nếu tất cả đều hợp lệ, tạo user
            var newUser = new User
            {
                Code = userRequest.Code,
                Password = ComputeSha256("123456"),
                FullName = userRequest.FullName,
                Dob = userRequest.Dob,
                Gender = userRequest.Gender,
                Email = userRequest.Email,
                PhoneNumber = userRequest.PhoneNumber,
                PlaceBirth = userRequest.PlaceBirth,
                Nation = userRequest.Nation,
                Religion = userRequest.Religion,
                EnrollmentDate = userRequest.EnrollmentDate,
                RoleId = userRequest.RoleId,
                AcademicYearId = userRequest.AcademicYearId,
                UserStatusId = userRequest.UserStatusId,
                ClassId = userRequest.ClassId,
                EntryType = userRequest.EntryType,
                AddressFull = userRequest.AddressFull,
                ProvinceCode = userRequest.ProvinceCode,
                DistrictCode = userRequest.DistrictCode,
                WardCode = userRequest.WardCode,
                Street = userRequest.Street,
                Active = userRequest.Active,
                AvatarUrl = _cloudinaryService.UploadBase64Async(userRequest.AvatarUrl).Result,
            };

            try
            {
                var createdUser = _userRepo.CreateUser(newUser);
                return ApiResponse<UserResponse>.Success(_mapper.Map<UserResponse>(createdUser));
            }
            catch (Exception ex)
            {
                return ApiResponse<UserResponse>.BadRequest(ex.Message);
            }
        }

        public ApiResponse<UserResponse> UpdateUser(int id, UserUpdateRequest userUpdateRequest)
        {
            var user = _userRepo.GetUserById(id);
            if (user == null)
            {
                return ApiResponse<UserResponse>.NotFound("Không tìm thấy người dùng để cập nhật");
            }

            // Kiểm tra RoleId có tồn tại không
            if (_roleRepo.GetRoleById(userUpdateRequest.RoleId) == null)
            {
                return ApiResponse<UserResponse>.BadRequest("RoleId không hợp lệ");
            }

            // Kiểm tra AcademicYearId có tồn tại không (nếu có nhập)
            if (userUpdateRequest.AcademicYearId.HasValue &&
                _academicYearRepo.GetAcademicYearById(userUpdateRequest.AcademicYearId.Value) == null)
            {
                return ApiResponse<UserResponse>.BadRequest("AcademicYearId không hợp lệ");
            }

            // Kiểm tra UserStatusId có tồn tại không (nếu có nhập)
            if (userUpdateRequest.UserStatusId.HasValue &&
                _userStatusRepo.GetUserStatusById(userUpdateRequest.UserStatusId.Value) == null)
            {
                return ApiResponse<UserResponse>.BadRequest("UserStatusId không hợp lệ");
            }

            // Kiểm tra ClassId có tồn tại không (nếu có nhập)
            if (userUpdateRequest.ClassId.HasValue &&
                _classRepo.GetClassById(userUpdateRequest.ClassId.Value) == null)
            {
                return ApiResponse<UserResponse>.BadRequest("ClassId không hợp lệ");
            }

            // Cập nhật thông tin người dùng
            user.FullName = userUpdateRequest.FullName;
            user.Email = userUpdateRequest.Email;
            user.PhoneNumber = userUpdateRequest.PhoneNumber;
            user.Dob = userUpdateRequest.Dob;
            user.Gender = userUpdateRequest.Gender;
            user.AddressFull = userUpdateRequest.AddressFull;
            user.Street = userUpdateRequest.Street;
            user.RoleId = userUpdateRequest.RoleId;
            user.AcademicYearId = userUpdateRequest.AcademicYearId;
            user.ProvinceCode = userUpdateRequest.ProvinceCode;
            user.DistrictCode = userUpdateRequest.DistrictCode;
            user.WardCode = userUpdateRequest.WardCode;
            user.UserStatusId = userUpdateRequest.UserStatusId;
            user.ClassId = userUpdateRequest.ClassId;
            user.EntryType = userUpdateRequest.EntryType;
            user.Active = userUpdateRequest.Active;
            user.AvatarUrl = _cloudinaryService.UploadBase64Async(userUpdateRequest.AvatarUrl).Result;

            try
            {
                var updatedUser = _userRepo.UpdateUser(user);
                return ApiResponse<UserResponse>.Success(_mapper.Map<UserResponse>(updatedUser));
            }
            catch (Exception ex)
            {
                return ApiResponse<UserResponse>.BadRequest(ex.Message);
            }
        }

        public ApiResponse<User> DeleteUser(int id)
        {
            var success = _userRepo.DeleteUser(id);
            return success
                ? ApiResponse<User>.Success()
                : ApiResponse<User>.NotFound("Không tìm thấy người dùng để xóa");
        }

        public ApiResponse<UserResponse> UpdateUserPassword(int userId, string newPassword)
        {
            var user = _userRepo.GetUserById(userId);
            if (user == null)
            {
                return ApiResponse<UserResponse>.NotFound("Không tìm thấy người dùng để cập nhật mật khẩu");
            }

            user.Password = ComputeSha256(newPassword);

            try
            {
                var updatedUser = _userRepo.UpdateUser(user);
                return ApiResponse<UserResponse>.Success(_mapper.Map<UserResponse>(updatedUser));
            }
            catch (Exception ex)
            {
                return ApiResponse<UserResponse>.BadRequest(ex.Message);
            }
        }

        public ApiResponse<UserResponse> ChangePassword(int userId, string currentPassword, string newPassword)
        {
            var user = _userRepo.GetUserById(userId);
            if (user == null)
            {
                return ApiResponse<UserResponse>.NotFound("Người dùng không tồn tại");
            }

            bool isValid = user.Password == ComputeSha256(currentPassword);
            if (!isValid)
            {
                return ApiResponse<UserResponse>.BadRequest("Mật khẩu hiện tại không đúng");
            }

            user.Password = ComputeSha256(newPassword);
            _userRepo.UpdateUser(user);

            return ApiResponse<UserResponse>.Success(_mapper.Map<UserResponse>(user));
        }


        public static string ComputeSha256(string input)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input + "ledang"));
            StringBuilder builder = new();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }

        // Lấy thông tin sinh viên theo ID
        public async Task<ApiResponse<StudentProcessResponse>> GetStudentById(int userId)
        {
            if (userId <= 0)
                return ApiResponse<StudentProcessResponse>.Fail("Mã người dùng không được để trống");
            else
            {
                var student = await _userRepo.GetStudentById(userId);
                if (student == null)
                    return ApiResponse<StudentProcessResponse>.Fail("Mã người dùng không phải là học viên");
                var students = await _userRepo.GetUsersByClassId(student.ClassId ?? 0);
                if (students == null || students.Count == 0)
                    return ApiResponse<StudentProcessResponse>.Fail("Không tìm thấy thông tin lớp học của học viên");
                var subjects = await _classSubjectRepo.GetClassSubjectsByClassId(student.ClassId ?? 0);
                if (subjects == null || subjects.Count == 0)
                    return ApiResponse<StudentProcessResponse>.Fail("Không tìm thấy thông tin môn học của lớp học");
                var response = _mapper.Map<StudentProcessResponse>(student);
                response.StudentQty = students.Count;
                response.SubjectQty = subjects.Count;
                return ApiResponse<StudentProcessResponse>.Success(_mapper.Map<StudentProcessResponse>(response));
            }

        }
    }
}
