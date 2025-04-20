using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;
using System;
using System.Collections.Generic;
using ISC_ELIB_SERVER.Services.Interfaces;

namespace ISC_ELIB_SERVER.Services
{
    public class TestsAttachmentService : ITestsAttachmentService
    {
        private readonly TestsAttachmentRepo _repository;
        private readonly IMapper _mapper;

        public TestsAttachmentService(TestsAttachmentRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // Lấy tất cả Attachment
        public ApiResponse<ICollection<TestsAttachmentResponse>> GetTestsAttachments(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetTestsAttachments().AsQueryable();

            query = sortColumn switch
            {
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(ts => ts.Id) : query.OrderBy(ts => ts.Id),
                _ => query.OrderBy(ts => ts.Id)
            };

            var totalItems = query.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var response = _mapper.Map<ICollection<TestsAttachmentResponse>>(result);

            return result.Any()
                 ? ApiResponse<ICollection<TestsAttachmentResponse>>.Success(response, page, pageSize, totalItems)
                 : ApiResponse<ICollection<TestsAttachmentResponse>>.NotFound("Không có dữ liệu");
        }

        // Lấy Attachment theo ID
        public ApiResponse<TestsAttachmentResponse> GetTestsAttachmentById(long id)
        {
            var attachment = _repository.GetTestsAttachmentById(id);
            if (attachment == null)
            {
                return ApiResponse<TestsAttachmentResponse>.NotFound("Không tồn tại.");
            }
            var response = _mapper.Map<TestsAttachmentResponse>(attachment);
            return ApiResponse<TestsAttachmentResponse>.Success(response);
        }

        // Tạo mới Attachment
        public ApiResponse<TestsAttachmentResponse> CreateTestsAttachment(TestsAttachmentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FileUrl))
            {
                return ApiResponse<TestsAttachmentResponse>.Error(new Dictionary<string, string[]>
                {
                    { "FileUrl", new[] { "File URL không được bỏ trống" } }
                });
            }

            var attachment = _mapper.Map<TestsAttachment>(request);

            var result = _repository.CreateTestsAttachment(attachment);
            if (result == null)
            {
                return ApiResponse<TestsAttachmentResponse>.Error(new Dictionary<string, string[]>
                {
                    { "Database", new[] { "Thêm mới thất bại" } }
                });
            }

            var response = _mapper.Map<TestsAttachmentResponse>(result);
            return ApiResponse<TestsAttachmentResponse>.Success(response);
        }

        // Cập nhật Attachment
        public ApiResponse<TestsAttachmentResponse> UpdateTestsAttachment(long id, TestsAttachmentRequest request)
        {
            if (request == null)
            {
                return ApiResponse<TestsAttachmentResponse>.Error(new Dictionary<string, string[]>
                {
                    { "Request", new[] { "Không thể là bỏ trống" } }
                });
            }

            if (string.IsNullOrWhiteSpace(request.FileUrl))
            {
                return ApiResponse<TestsAttachmentResponse>.Error(new Dictionary<string, string[]>
                {
                    { "FileUrl", new[] { "File URL không được bỏ trống" } }
                });
            }

            // Kiểm tra xem attachment có tồn tại không
            var existingAttachment = _repository.GetTestsAttachmentById(id);
            if (existingAttachment == null)
            {
                return ApiResponse<TestsAttachmentResponse>.NotFound("Không tồn tại.");
            }

            try
            {
                // Cập nhật dữ liệu hợp lệ
                existingAttachment.FileUrl = request.FileUrl;
                existingAttachment.SubmissionId = request.SubmissionId;

                // Cập nhật trong DB
                var updatedAttachment = _repository.UpdateTestsAttachment(existingAttachment);

                if (updatedAttachment == null)
                {
                    return ApiResponse<TestsAttachmentResponse>.Error(new Dictionary<string, string[]>
                    {
                        { "Database", new[] { "Cập nhật thất bại" } }
                    });
                }

                var response = _mapper.Map<TestsAttachmentResponse>(updatedAttachment);
                return ApiResponse<TestsAttachmentResponse>.Success(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Update thất bại: {ex.Message}");
                return ApiResponse<TestsAttachmentResponse>.Error(new Dictionary<string, string[]>
                {
                    { "Exception", new[] { "Có lỗi xảy ra trong quá trình cập nhật." } }
                });
            }
        }

        // Xóa Attachment
        public ApiResponse<TestsAttachmentResponse> DeleteTestsAttachment(long id)
        {
            // Kiểm tra xem attachment có tồn tại không
            var existingAttachment = _repository.GetTestsAttachmentById(id);
            if (existingAttachment == null)
            {
                return ApiResponse<TestsAttachmentResponse>.NotFound("Không tồn tại.");
            }

            try
            {
                // Thực hiện xóa mềm
                var success = _repository.DeleteTestsAttachment(id);

                if (success)
                {
                    return ApiResponse<TestsAttachmentResponse>.Success();
                }

                return ApiResponse<TestsAttachmentResponse>.Error(new Dictionary<string, string[]>
                {
                    { "Exception", new[] { "Không có thay đổi nào được thực hiện." } }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Xóa thất bại: {ex.Message}");
                return ApiResponse<TestsAttachmentResponse>.Error(new Dictionary<string, string[]>
                {
                    { "Exception", new[] { "Có lỗi xảy ra trong quá trình xóa." } }
                });
            }
        }

    }
}
