using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Requests.ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Services
{
    public class TransferSchoolService : ITransferSchoolService
    {
        private readonly TransferSchoolRepo _repo;
        private readonly IMapper _mapper;
        private readonly isc_dbContext _context;
        private readonly GhnService _ghnService;
        private readonly StudentInfoRepo _studentRepository;
        private readonly UserRepo _userRepository;

        public TransferSchoolService(TransferSchoolRepo repo, IMapper mapper, isc_dbContext context, GhnService ghnService, StudentInfoRepo studentRepository, UserRepo userRepository)
        {
            _repo = repo;
            _mapper = mapper;
            _context = context;
            _ghnService = ghnService;
            _studentRepository = studentRepository;
            _userRepository = userRepository;
        }

        public ApiResponse<TransferSchoolResponse> GetTransferSchoolByStudentCode(string studentCode)
        {
            try
            {
                var transferSchool = _repo.GetTransferSchoolByStudentCode(studentCode);

                if (transferSchool == null)
                {
                    return ApiResponse<TransferSchoolResponse>.Fail("Không tìm thấy dữ liệu chuyển trường.");
                }

                return ApiResponse<TransferSchoolResponse>.Success(transferSchool);
            }
            catch (Exception ex)
            {
                return ApiResponse<TransferSchoolResponse>.Fail(ex.Message);
            }
        }

        

        public ApiResponse<TransferSchoolResponse> CreateTransferSchool(TransferSchoolRequest request)
        {
            try
            {
                // Tìm StudentId từ StudentCode
                var student = _context.Users.FirstOrDefault(u => u.Code == request.StudentCode);
                if (student == null)
                {
                    return ApiResponse<TransferSchoolResponse>.Fail("Không tìm thấy học sinh với mã StudentCode đã cung cấp.");
                }

                var studentId = student.Id;

                // Kiểm tra xem StudentId đã tồn tại trong bảng TransferSchool chưa
                var isStudentIdExist = _context.TransferSchools.Any(ts => ts.StudentId == studentId);
                if (isStudentIdExist)
                {
                    return ApiResponse<TransferSchoolResponse>.Fail("Học sinh này đã có trong danh sách chuyển trường.");
                }

                // Lưu thông tin chuyển trường
                var transferSchool = new TransferSchool
                {
                    StudentId = studentId,
                    TransferSchoolDate = DateTime.SpecifyKind(request.TransferSchoolDate, DateTimeKind.Unspecified),
                    TransferToSchool = request.TransferToSchool,
                    SchoolAddress = request.SchoolAddress,
                    Reason = request.Reason,
                    AttachmentName = request.AttachmentName,
                    AttachmentPath = request.AttachmentPath,
                    SemesterId = request.SemesterId,
                    UserId = request.UserId,  // Gán userId từ request (lấy từ token)
                    Active = true
                };

                var created = _repo.CreateTransferSchool(transferSchool);

                // Lấy thông tin địa phương từ GHN Service
                var (provinceName, districtName, wardName) = _ghnService.GetLocationName(
                    request.ProvinceCode ?? 0,
                    request.DistrictCode ?? 0,
                    "" // Không cần WardCode
                ).Result;

                // Trả về thông tin sau khi lưu
                var transferSchoolRepo = new TransferSchoolResponse
                {
                    StudentId = created.StudentId,
                    TransferSchoolDate = created.TransferSchoolDate,
                    TransferToSchool = created.TransferToSchool,
                    Reason = created.Reason,
                    AttachmentName = created.AttachmentName,
                    AttachmentPath = created.AttachmentPath,
                    StatusCode = 200
                };

                return ApiResponse<TransferSchoolResponse>.Success(_mapper.Map<TransferSchoolResponse>(transferSchoolRepo));
            }
            catch (Exception ex)
            {
                return ApiResponse<TransferSchoolResponse>.Fail(ex.Message);
            }
        }

        public ApiResponse<TransferSchoolResponse> UpdateTransferSchool(string studentCode, TransferSchoolRequest request)
        {
            try
            {
                // Tìm StudentId từ StudentCode**
                var student = _context.Users.FirstOrDefault(u => u.Code == studentCode);
                if (student == null)
                {
                    return ApiResponse<TransferSchoolResponse>.Fail("Không tìm thấy học sinh với mã StudentCode đã cung cấp.");
                }

                var studentId = student.Id;

                //Tìm bản ghi TransferSchool cần cập nhật**
                var existingTransfer = _context.TransferSchools.FirstOrDefault(ts => ts.StudentId == studentId);
                if (existingTransfer == null)
                {
                    return ApiResponse<TransferSchoolResponse>.Fail("Không tìm thấy dữ liệu chuyển trường để cập nhật.");
                }

                //Cập nhật thông tin chuyển trường**
                existingTransfer.TransferSchoolDate = DateTime.SpecifyKind(request.TransferSchoolDate, DateTimeKind.Unspecified);
                existingTransfer.TransferToSchool = request.TransferToSchool;
                existingTransfer.SchoolAddress = request.SchoolAddress;
                existingTransfer.Reason = request.Reason;
                existingTransfer.AttachmentName = request.AttachmentName;
                existingTransfer.AttachmentPath = request.AttachmentPath;
                existingTransfer.SemesterId = request.SemesterId;
                existingTransfer.UserId = request.UserId;  // Lưu userId từ token

                //*Lưu thay đổi vào DB**
                _context.SaveChanges();

                return ApiResponse<TransferSchoolResponse>.Success(_mapper.Map<TransferSchoolResponse>(existingTransfer));
            }
            catch (Exception ex)
            {
                return ApiResponse<TransferSchoolResponse>.Fail(ex.Message);
            }
        }

        public ApiResponse<TransferSchoolResponse> GetTransferSchoolByStudentId(int studentId)
        {
            try
            {
                var result = _repo.GetTransferSchoolByStudentId(studentId);
                if (result == null)
                {
                    return ApiResponse<TransferSchoolResponse>.Fail("Không tìm thấy thông tin chuyển trường cho học sinh này.");
                }

                return ApiResponse<TransferSchoolResponse>.Success(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<TransferSchoolResponse>.Fail(ex.Message);
            }
        }




        public ApiResponse<ICollection<TeacherInfoResponses>> GetTransferSchoolList(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            throw new NotImplementedException();
        }

        public ApiResponse<TransferSchoolResponse> DeleteTransferSchool(int studentId)
        {
            // Gọi repository để thực hiện xóa và nhận kết quả trả về
            return _repo.DeleteTransferSchool(studentId);
        }
    }
}
