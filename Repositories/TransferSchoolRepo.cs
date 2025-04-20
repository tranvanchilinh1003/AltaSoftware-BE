using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ISC_ELIB_SERVER.Models;
using System.Numerics;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests.ISC_ELIB_SERVER.DTOs.Requests;


namespace ISC_ELIB_SERVER.Repositories
{
    public class TransferSchoolRepo
    {
        private readonly isc_dbContext _context;
        private readonly GhnService _ghnService;
        public TransferSchoolRepo(isc_dbContext context, GhnService ghnService)
        {
            _context = context;
            _ghnService = ghnService;
        }

        /// <summary>
        /// Lấy danh sách học sinh đã chuyển trường.
        /// </summary>
        /// 
        public List<object> GetTransferSchoolList(string? search)
        {
            var query = _context.TransferSchools
                .Where(ts => ts.Active)
                .Join(_context.Users, ts => ts.StudentId, u => u.Id, (ts, u) => new { ts, u });

            // Nếu có từ khóa search thì lọc theo tên
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.u.FullName.Contains(search));
            }

            return query
                .Join(_context.Semesters, tsu => tsu.ts.SemesterId, s => s.Id, (tsu, s) => new { tsu, s })
                .Join(_context.Classes, tsuc => tsuc.tsu.u.ClassId, c => c.Id, (tsuc, c) => new { tsuc, c })
                .Join(_context.GradeLevels, tsucg => tsucg.c.GradeLevelId, gl => gl.Id, (tsucg, gl) => new { tsucg, gl })
                .Select(res => new
                {
                    StudentId = res.tsucg.tsuc.tsu.ts.StudentId,
                    FullName = res.tsucg.tsuc.tsu.u.FullName,
                    Code = res.tsucg.tsuc.tsu.u.Code,
                    DateOfBirth = res.tsucg.tsuc.tsu.u.Dob,
                    Gender = res.tsucg.tsuc.tsu.u.Gender == true ? "Nam" : "Nữ",
                    TransferDate = res.tsucg.tsuc.tsu.ts.TransferSchoolDate,
                    TransferSemester = res.tsucg.tsuc.s.Name,
                    TransferToSchool = res.tsucg.tsuc.tsu.ts.TransferToSchool,
                    GradeLevel = res.gl.Name,
                    SemesterStart = res.tsucg.tsuc.s.StartTime,
                    SemesterEnd = res.tsucg.tsuc.s.EndTime
                })
                .Distinct()
                .ToList<object>();
        }



        /// <summary>
        /// Lấy thông tin chi tiết học sinh chuyển trường theo StudentId.
        /// </summary>
        public TransferSchoolResponse? GetTransferSchoolByStudentId(int studentId)
        {
            try
            {
                var transferSchool = _context.TransferSchools
                    .Where(ts => ts.StudentId == studentId && ts.Active)
                    .Join(_context.Users, ts => ts.StudentId, u => u.Id, (ts, u) => new { ts, u })
                    .Join(_context.Semesters, tsu => tsu.ts.SemesterId, s => s.Id, (tsu, s) => new
                    {
                        StudentId = tsu.ts.StudentId,
                        SemesterId = s.Id,
                        StudentCode = tsu.u.Code ?? "Không có mã học viên",
                        FullName = tsu.u.FullName ?? "Không có tên",
                        TransferSemester = s.Name ?? "Không có học kỳ",
                        TransferSchoolDate = tsu.ts.TransferSchoolDate ?? DateTime.MinValue,
                        TransferToSchool = tsu.ts.TransferToSchool ?? "Không có thông tin trường chuyển đến",
                        ProvinceCode = tsu.u.ProvinceCode ?? 0,
                        DistrictCode = tsu.u.DistrictCode ?? 0,
                        Reason = tsu.ts.Reason ?? "Không có lý do",
                        AttachmentName = tsu.ts.AttachmentName ?? "Không có tệp đính kèm",
                        AttachmentPath = tsu.ts.AttachmentPath ?? string.Empty
                    })
                    .FirstOrDefault();

                if (transferSchool == null)
                    return null;

                return new TransferSchoolResponse
                {
                    StudentId = transferSchool.StudentId,
                    SemesterId = transferSchool.SemesterId,
                    FullName = transferSchool.FullName,
                    StudentCode = transferSchool.StudentCode,
                    TransferSchoolDate = transferSchool.TransferSchoolDate,
                    TransferToSchool = transferSchool.TransferToSchool,
                    TransferSemester = transferSchool.TransferSemester,
                    Reason = transferSchool.Reason,
                    ProvinceCode = transferSchool.ProvinceCode,
                    DistrictCode = transferSchool.DistrictCode,
                    AttachmentName = transferSchool.AttachmentName,
                    AttachmentPath = transferSchool.AttachmentPath,
                    StatusCode = 200
                };
            }
            catch
            {
                return null;
            }
        }


        public TransferSchoolResponse? GetTransferSchoolByStudentCode(string studentCode)
        {
            if (string.IsNullOrWhiteSpace(studentCode))
                return null;

            try 
            {
                var user = _context.Users
                    .FirstOrDefault(u => u.Code != null && u.Code.ToLower() == studentCode.Trim().ToLower());

                if (user == null)
                    return null;

                return GetTransferSchoolByUserId(user.Id);
            }
            catch 
            {
                return null;
            }
        }

        public TransferSchoolResponse? GetTransferSchoolByUserId(int userId)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                    return null;

                var transferSchool = _context.TransferSchools
                    .Where(ts => ts.StudentId == userId && ts.Active) // studentId = userId
                    .Join(_context.Semesters,
                          ts => ts.SemesterId,
                          s => s.Id,
                          (ts, s) => new { ts, s })
                    .Select(res => new
                    {
                        StudentId = res.ts.StudentId,
                        SemesterId = res.s.Id,
                        StudentCode = user.Code ?? "Không có mã học viên",
                        FullName = user.FullName ?? "Không có tên",
                        TransferSchoolDate = res.ts.TransferSchoolDate ?? DateTime.MinValue,
                        TransferToSchool = res.ts.TransferToSchool ?? "Không có thông tin trường chuyển đến",
                        TransferSemester = res.s.Name ?? "Không có học kỳ",
                        Reason = res.ts.Reason ?? "Không có lý do",
                        ProvinceCode = user.ProvinceCode ?? 0,
                        DistrictCode = user.DistrictCode ?? 0,
                        AttachmentName = res.ts.AttachmentName ?? "Không có tệp đính kèm",
                        AttachmentPath = res.ts.AttachmentPath ?? string.Empty
                    })
                    .FirstOrDefault();

                if (transferSchool == null)
                    return null;

                return new TransferSchoolResponse
                {
                    StudentId = transferSchool.StudentId,
                    SemesterId = transferSchool.SemesterId,
                    FullName = transferSchool.FullName,
                    StudentCode = transferSchool.StudentCode,
                    TransferSchoolDate = transferSchool.TransferSchoolDate,
                    TransferToSchool = transferSchool.TransferToSchool,
                    TransferSemester = transferSchool.TransferSemester,
                    Reason = transferSchool.Reason,
                    ProvinceCode = transferSchool.ProvinceCode,
                    DistrictCode = transferSchool.DistrictCode,
                    AttachmentName = transferSchool.AttachmentName,
                    AttachmentPath = transferSchool.AttachmentPath,
                    StatusCode = 200
                };
            }
            catch
            {
                return null;
            }
        }


        public TransferSchool CreateTransferSchool(TransferSchool entity)
        {

            try
            {
                _context.TransferSchools.Add(entity);
                _context.SaveChanges();
                return entity;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Lưu dữ liệu thất bại! Chi tiết: " + ex.InnerException?.Message);
            }

        }


        /// <summary>
        /// Thêm mới thông tin chuyển trường.
        /// </summary>
        public TransferSchool? GetTransferSchoolById(int id)
        {
            return _context.TransferSchools
                .FirstOrDefault(ts => ts.Id == id && ts.Active);
        }


        /// <summary>
        /// Cập nhật thông tin chuyển trường.
        /// </summary>
        public TransferSchoolResponse? GetByStudentId(int studentId)
        {
            try
            {
                return _context.TransferSchools
                    .Where(ts => ts.StudentId == studentId && ts.Active)
                    .Join(_context.Users,
                          ts => ts.StudentId,
                          u => u.Id,
                          (ts, u) => new { ts, u })
                    .Join(_context.StudentInfos,
                          tsu => tsu.u.Id,
                          si => si.UserId,
                          (tsu, si) => new { tsu.ts, tsu.u, si })
                    .Join(_context.Semesters,
                          res => res.ts.SemesterId,
                          s => s.Id,
                          (res, s) => new TransferSchoolResponse
                          {
                              StudentId = studentId,  
                              FullName = res.u.FullName,
                              StudentCode = res.u.Code,
                              TransferSchoolDate = res.ts.TransferSchoolDate,
                              TransferToSchool = res.ts.TransferToSchool ?? string.Empty,
                              TransferSemester = s.Name,
                              SemesterId = s.Id,
                              Reason = res.ts.Reason,
                              ProvinceCode = res.u.ProvinceCode ?? 0,
                              DistrictCode = res.u.DistrictCode ?? 0,
                              AttachmentName = res.ts.AttachmentName,
                              AttachmentPath = res.ts.AttachmentPath ?? string.Empty,
                              StatusCode = 200
                          })
                    .FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public TransferSchool UpdateTransferSchool(TransferSchool transferSchool)
        {
            try
            {
                _context.TransferSchools.Update(transferSchool);
                _context.SaveChanges();
                return transferSchool;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Lỗi khi cập nhật dữ liệu: {ex.InnerException?.Message}");
            }
        }

        public ApiResponse<TransferSchoolResponse> DeleteTransferSchool(int studentId)
        {
            try
            {
                var transferSchool = _context.TransferSchools
                    .FirstOrDefault(ts => ts.StudentId == studentId && ts.Active);  // Kiểm tra Active nếu cần

                if (transferSchool == null)
                {
                    return ApiResponse<TransferSchoolResponse>.Fail("Không tìm thấy học sinh để xóa.");
                }

                _context.TransferSchools.Remove(transferSchool);
                _context.SaveChanges(); // Lưu thay đổi

                var transferSchoolResponse = new TransferSchoolResponse
                {
                    // Mapping các dữ liệu từ transferSchool sang TransferSchoolResponse
                    StudentId = transferSchool.StudentId,
                   
                    // Thêm các trường khác nếu cần
                };

                return ApiResponse<TransferSchoolResponse>.Success(transferSchoolResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<TransferSchoolResponse>.Fail("Xóa thất bại: " + ex.Message);
            }
        }


    }
}
