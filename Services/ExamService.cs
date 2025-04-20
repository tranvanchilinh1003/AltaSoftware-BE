using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models.Responses;
using ISC_ELIB_SERVER.Repositories;
using AutoMapper;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Services.Interfaces;

namespace ISC_ELIB_SERVER.Services
{
    public class ExamService : IExamService
    {
        private readonly ExamRepo _repository;
        private readonly IMapper _mapper;

        public ExamService(ExamRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<ExamResponse>> GetExams(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetExams().AsQueryable();

            query = sortColumn switch
            {
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(e => e.Id) : query.OrderBy(e => e.Id),
                "Name" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(e => e.Name) : query.OrderBy(e => e.Name),
                "ExamDate" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(e => e.ExamDate) : query.OrderBy(e => e.ExamDate),
                _ => query.OrderBy(e => e.Id)
            };

            var totalItems = query.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var response = _mapper.Map<ICollection<ExamResponse>>(result);

            return result.Any()
                  ? ApiResponse<ICollection<ExamResponse>>.Success(response, page, pageSize, totalItems)
                  : ApiResponse<ICollection<ExamResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<ExamResponse> GetExamById(long id)
        {
            var exam = _repository.GetExamById(id);
            if (exam == null)
            {
                return ApiResponse<ExamResponse>.NotFound();
            }
            return ApiResponse<ExamResponse>.Success(_mapper.Map<ExamResponse>(exam));
        }

        public ApiResponse<ICollection<ExamResponse>> GetExamByName(string name)
        {
            var exams = _repository.GetExamByName(name); // Lấy danh sách các kỳ thi

            if (exams == null || !exams.Any()) // Kiểm tra nếu danh sách rỗng
            {
                return ApiResponse<ICollection<ExamResponse>>.NotFound();
            }

            return ApiResponse<ICollection<ExamResponse>>.Success(_mapper.Map<ICollection<ExamResponse>>(exams));
        }

        public ApiResponse<ExamResponse> CreateExam(ExamRequest request)
        {
            var exam = _mapper.Map<Exam>(request);
            exam.ExamDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
            //exam.File = "/uploads/exams/" + request.File;

            if (request.File != null && request.File.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/exams");
                Directory.CreateDirectory(uploadsFolder);

                string fileName = Guid.NewGuid().ToString() + "_" + request.File.FileName;
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    request.File.CopyTo(fileStream);
                }

                exam.File = $"/uploads/exams/{fileName}";
            }
            else
            {
                exam.File = null;
            }

            var createdExam = _repository.CreateExam(exam);

            if (createdExam != null)
            {
                var response = _mapper.Map<ExamResponse>(createdExam);
                return ApiResponse<ExamResponse>.Success(response);
            }

            return ApiResponse<ExamResponse>.Error(new Dictionary<string, string[]>
            {
                { "Error", new[] { "Tạo bài thi thất bại" } }
            });
        }

        public ApiResponse<ExamResponse> UpdateExam(long id, ExamRequest request)
        {
            var existingExam = _repository.GetExamById(id);
            if (existingExam == null)
            {
                return ApiResponse<ExamResponse>.Error(new Dictionary<string, string[]>
                {
                    { "Error", new[] { "Bài thi không tồn tại" } }
                });
            }

            // Lưu lại đường dẫn file cũ
            string? oldFilePath = existingExam.File;

            // Cập nhật các thuộc tính khác từ request, nhưng không ghi đè File nếu request.File là null
            _mapper.Map(request, existingExam);

            if (request.File != null && request.File.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/exams");
                Directory.CreateDirectory(uploadsFolder);

                // Xóa file cũ nếu có
                if (!string.IsNullOrEmpty(oldFilePath))
                {
                    string oldFileFullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldFilePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldFileFullPath))
                    {
                        System.IO.File.Delete(oldFileFullPath);
                    }
                }

                // Lưu file mới
                string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(request.File.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    request.File.CopyTo(fileStream);
                }

                // Cập nhật đường dẫn file mới
                existingExam.File = $"/uploads/exams/{fileName}";
            }
            else
            {
                // Nếu không có file mới, giữ lại đường dẫn file cũ
                existingExam.File = oldFilePath;
            }

            // Lưu thay đổi
            var updatedExam = _repository.UpdateExam(existingExam);
            if (updatedExam != null)
            {
                var response = _mapper.Map<ExamResponse>(updatedExam);
                return ApiResponse<ExamResponse>.Success(response);
            }

            return ApiResponse<ExamResponse>.Error(new Dictionary<string, string[]>
            {
                { "Error", new[] { "Cập nhật bài thi thất bại" } }
            });
        }

        public ApiResponse<ExamResponse> DeleteExam(long id)
        {
            // Kiểm tra xem exam có tồn tại không
            var existingExam = _repository.GetExamById(id);
            if (existingExam == null)
            {
                return ApiResponse<ExamResponse>.NotFound("Không tồn tại.");
            }

            try
            {
                // Thực hiện xóa mềm
                var success = _repository.DeleteExam(id);

                if (success)
                {
                    return ApiResponse<ExamResponse>.Success();
                }

                return ApiResponse<ExamResponse>.Error(new Dictionary<string, string[]>
                {
                    { "Exception", new[] { "Không có thay đổi nào được thực hiện." } }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Xóa thất bại: {ex.Message}");
                return ApiResponse<ExamResponse>.Error(new Dictionary<string, string[]>
                {
                    { "Exception", new[] { "Có lỗi xảy ra trong quá trình xóa." } }
                });
            }
        }

    }
}
