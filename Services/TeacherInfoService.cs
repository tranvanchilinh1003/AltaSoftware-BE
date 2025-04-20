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
    public class TeacherInfoService : ITeacherInfoService
    {
        private readonly TeacherInfoRepo _repository;
        private readonly IMapper _mapper;

        public TeacherInfoService(TeacherInfoRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<TeacherInfoResponses>> GetTeacherInfos(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetAllTeacherInfo().AsQueryable();

            // Tìm kiếm theo một số thuộc tính ví dụ: CCCD và AddressFull
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(t =>
                    (t.Cccd != null && t.Cccd.ToLower().Contains(search.ToLower())) ||
                    (t.AddressFull != null && t.AddressFull.ToLower().Contains(search.ToLower()))
                );
            }

            // Sắp xếp theo các cột được chỉ định
            query = sortColumn switch
            {
                "Cccd" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(t => t.Cccd) : query.OrderBy(t => t.Cccd),
                "IssuedDate" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(t => t.IssuedDate) : query.OrderBy(t => t.IssuedDate),
                _ => sortOrder.ToLower() == "desc" ? query.OrderByDescending(t => t.Id) : query.OrderBy(t => t.Id)
            };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var response = _mapper.Map<ICollection<TeacherInfoResponses>>(result);

            return result.Any()
                ? ApiResponse<ICollection<TeacherInfoResponses>>.Success(response)
                : ApiResponse<ICollection<TeacherInfoResponses>>.NotFound("Không có dữ liệu TeacherInfo");
        }

        public ApiResponse<TeacherInfoResponses> GetTeacherInfoById(int id)
        {
            var teacherInfo = _repository.GetTeacherInfoById(id);
            return teacherInfo != null
                ? ApiResponse<TeacherInfoResponses>.Success(_mapper.Map<TeacherInfoResponses>(teacherInfo))
                : ApiResponse<TeacherInfoResponses>.NotFound($"Không tìm thấy TeacherInfo với ID #{id}");
        }

        public ApiResponse<TeacherInfoResponses> GetTeacherInfoByCode(string code)
        {
            // Giả sử thuộc tính CCCD được dùng làm mã code
            var teacherInfo = _repository.GetAllTeacherInfo()
                                         .FirstOrDefault(t => t.Cccd != null && t.Cccd.ToLower() == code.ToLower());
            return teacherInfo != null
                ? ApiResponse<TeacherInfoResponses>.Success(_mapper.Map<TeacherInfoResponses>(teacherInfo))
                : ApiResponse<TeacherInfoResponses>.NotFound($"Không tìm thấy TeacherInfo với mã {code}");
        }

        public ApiResponse<TeacherInfoResponses> CreateTeacherInfo(TeacherInfoRequest teacherInfoRequest)
        {
            // Kiểm tra xem TeacherInfo với mã (CCCD) đã tồn tại hay chưa
            var existing = _repository.GetAllTeacherInfo()
                                      .FirstOrDefault(t => t.Cccd != null && t.Cccd.ToLower() == teacherInfoRequest.Cccd?.ToLower());
            if (existing != null)
            {
                return ApiResponse<TeacherInfoResponses>.Conflict("TeacherInfo với mã này đã tồn tại");
            }

            var teacherInfo = _mapper.Map<TeacherInfo>(teacherInfoRequest);

            var created = _repository.CreateTeacherInfo(teacherInfo);

            try
            {

                var createdTeacherInfo = _mapper.Map<TeacherInfoResponses>(created);

                return ApiResponse<TeacherInfoResponses>.Success(createdTeacherInfo);
            }
            catch
            {
                return ApiResponse<TeacherInfoResponses>.BadRequest("Lỗi, xem lại khóa ngoại");
            }

        }

        public ApiResponse<TeacherInfoResponses> UpdateTeacherInfo(int id, TeacherInfoRequest teacherInfoRequest)
        {
            var teacherInfo = _repository.GetTeacherInfoById(id);
            if (teacherInfo == null)
            {
                return ApiResponse<TeacherInfoResponses>.NotFound("Không tìm thấy TeacherInfo để cập nhật");
            }

            _mapper.Map(teacherInfoRequest, teacherInfo);
            _repository.UpdateTeacherInfo(teacherInfo);
            var updatedTeacherInfo = _mapper.Map<TeacherInfoResponses>(teacherInfo);

            return ApiResponse<TeacherInfoResponses>.Success(updatedTeacherInfo);
        }

        public ApiResponse<TeacherInfoResponses> DeleteTeacherInfo(int id)
        {
            var teacherInfo = _repository.GetTeacherInfoById(id);
            if (teacherInfo == null)
            {
                return ApiResponse<TeacherInfoResponses>.NotFound("Không tìm thấy TeacherInfo để xóa");
            }

            _repository.DeleteTeacherInfo(id);
            return ApiResponse<TeacherInfoResponses>.Success();
        }
    }
}
