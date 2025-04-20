using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ISC_ELIB_SERVER.Services
{
    public class StudentInfoService : IStudentInfoService
    {
        private readonly StudentInfoRepo _repository;
        private readonly IMapper _mapper;

        public StudentInfoService(StudentInfoRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<StudentInfoResponses>> GetStudentInfos(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetAllStudentInfo().AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s =>
                    (s.GuardianName != null && s.GuardianName.ToLower().Contains(search.ToLower())) ||
                    (s.GuardianPhone != null && s.GuardianPhone.ToLower().Contains(search.ToLower()))
                );
            }

            query = sortColumn switch
            {
                "GuardianName" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(s => s.GuardianName) : query.OrderBy(s => s.GuardianName),
                "GuardianDob" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(s => s.GuardianDob) : query.OrderBy(s => s.GuardianDob),
                _ => sortOrder.ToLower() == "desc" ? query.OrderByDescending(s => s.Id) : query.OrderBy(s => s.Id)
            };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var response = _mapper.Map<ICollection<StudentInfoResponses>>(result);

            return result.Any()
                ? ApiResponse<ICollection<StudentInfoResponses>>.Success(response)
                : ApiResponse<ICollection<StudentInfoResponses>>.NotFound("Không có dữ liệu StudentInfo");
        }

        public ApiResponse<StudentInfoResponses> GetStudentInfoById(int userId)
        {
            var studentInfo = _repository.GetStudentInfoById(userId);
            return studentInfo != null
                ? ApiResponse<StudentInfoResponses>.Success(_mapper.Map<StudentInfoResponses>(studentInfo))
                : ApiResponse<StudentInfoResponses>.NotFound($"Không tìm thấy StudentInfo với ID #{userId}");
        }

        public ApiResponse<StudentInfoResponses> CreateStudentInfo(StudentInfoRequest studentInfoRequest)
        {
            var studentInfo = _mapper.Map<StudentInfo>(studentInfoRequest);

            studentInfo.GuardianDob = DateTime.SpecifyKind(studentInfo.GuardianDob, DateTimeKind.Unspecified);

            _repository.AddStudentInfo(studentInfo);
            var createdStudentInfo = _mapper.Map<StudentInfoResponses>(studentInfo);

            return ApiResponse<StudentInfoResponses>.Success(createdStudentInfo);
        }

        public ApiResponse<StudentInfoResponses> UpdateStudentInfo(int id, StudentInfoRequest studentInfoRequest)
        {
            var studentInfo = _repository.GetStudentInfoById(id);
            if (studentInfo == null)
            {
                return ApiResponse<StudentInfoResponses>.NotFound("Không tìm thấy StudentInfo để cập nhật");
            }

            _mapper.Map(studentInfoRequest, studentInfo);
            _repository.UpdateStudentInfo(studentInfo);
            var updatedStudentInfo = _mapper.Map<StudentInfoResponses>(studentInfo);

            return ApiResponse<StudentInfoResponses>.Success(updatedStudentInfo);
        }

        public ApiResponse<StudentInfoResponses> DeleteStudentInfo(int id)
        {
            var studentInfo = _repository.GetStudentInfoById(id);
            if (studentInfo == null)
            {
                return ApiResponse<StudentInfoResponses>.NotFound("Không tìm thấy StudentInfo để xóa");
            }

            _repository.DeleteStudentInfo(id);
            return ApiResponse<StudentInfoResponses>.Success();
        }

        // Thêm phương thức lấy danh sách học viên theo UserId
        public ApiResponse<ICollection<StudentInfoResponses>> GetStudentsByUserId(int userId)
        {
            var studentInfos = _repository.GetStudentInfosByUserId(userId);
            if (studentInfos == null || !studentInfos.Any())
            {
                return ApiResponse<ICollection<StudentInfoResponses>>.NotFound($"Không tìm thấy thông tin học viên với UserId #{userId}");
            }

            var response = _mapper.Map<ICollection<StudentInfoResponses>>(studentInfos);
            return ApiResponse<ICollection<StudentInfoResponses>>.Success(response);
        }

        // Thêm phương thức lấy danh sách học viên theo lớp với thông tin đầy đủ
        public ApiResponse<ICollection<StudentInfoClassResponse>> GetStudentsByClass(int classId)
        {
            var studentInfos = _repository.GetStudentsByClass(classId);

            if (studentInfos == null || !studentInfos.Any())
            {
                return ApiResponse<ICollection<StudentInfoClassResponse>>.NotFound($"Không tìm thấy sinh viên cho lớp ID #{classId}");
            }

            var response = _mapper.Map<ICollection<StudentInfoClassResponse>>(studentInfos);
            return ApiResponse<ICollection<StudentInfoClassResponse>>.Success(response);
        }

        // Thêm phương thức lấy danh sách học viên theo user figma
        public ApiResponse<ICollection<StudentInfoUserResponse>> GetAllStudents()
        {
            var studentInfos = _repository.GetAllStudents();
            if (studentInfos == null || !studentInfos.Any())
            {
                return ApiResponse<ICollection<StudentInfoUserResponse>>.NotFound("Không có học viên nào.");
            }

            var response = _mapper.Map<ICollection<StudentInfoUserResponse>>(studentInfos);
            return ApiResponse<ICollection<StudentInfoUserResponse>>.Success(response);
        }

    }
}
