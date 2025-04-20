using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;
using System.Linq;

namespace ISC_ELIB_SERVER.Services
{
    public class TeacherListService : ITeacherListService
    {
        private readonly TeacherListRepo _repository;
        private readonly IMapper _mapper;

        public TeacherListService(TeacherListRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<TeacherListResponse> GetTeacherListById(int id)
        {
            var teacherInfo = _repository.GetTeacherListById(id);
            return teacherInfo != null
                ? ApiResponse<TeacherListResponse>.Success(_mapper.Map<TeacherListResponse>(teacherInfo))
                : ApiResponse<TeacherListResponse>.NotFound($"Không tìm thấy giáo viên với ID #{id}");
        }

        public ApiResponse<ICollection<TeacherListResponse>> GetTeacherLists(int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder)
        {
            // Gán giá trị mặc định nếu tham số null
            int currentPage = page ?? 1;
            int currentPageSize = pageSize ?? 10;

            var query = _repository.GetAllTeacherList().AsQueryable();

            // Lọc chỉ lấy những người dùng có vai trò là giáo viên
            query = query.Where(t => t.User != null && t.User.Role != null && t.User.Role.Id == 2);

            // Thực hiện tìm kiếm nếu có
            if (!string.IsNullOrEmpty(search))
            {
                var lowerSearch = search.ToLower();
                query = query.Where(t =>
                    t.User != null && (
                        (t.User.FullName != null && t.User.FullName.ToLower().Contains(lowerSearch)) ||
                        (t.User.Code != null && t.User.Code.ToLower().Contains(lowerSearch))
                    )
                );
            }

            // Sắp xếp theo cột được chỉ định
            query = sortColumn switch
            {
                "TeacherCode" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(t => t.User.Code) : query.OrderBy(t => t.User.Code),
                "FullName" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(t => t.User.FullName) : query.OrderBy(t => t.User.FullName),
                _ => sortOrder.ToLower() == "desc" ? query.OrderByDescending(t => t.Id) : query.OrderBy(t => t.Id)
            };

            // Tính tổng số bản ghi
            var totalItems = query.Count();

            // Phân trang
            var result = query.Skip((currentPage - 1) * currentPageSize)
                              .Take(currentPageSize)
                              .ToList();

            var response = _mapper.Map<ICollection<TeacherListResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<TeacherListResponse>>.Success(response, currentPage, currentPageSize, totalItems)
                : ApiResponse<ICollection<TeacherListResponse>>.NotFound("No data found for TeacherList");
        }
    }
}
