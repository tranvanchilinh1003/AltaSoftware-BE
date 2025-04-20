using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;

namespace ISC_ELIB_SERVER.Services
{
    public class ThemesService : IThemesService
    {
        private readonly ThemesRepo _repository;
        private readonly IMapper _mapper;

        public ThemesService(ThemesRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public ApiResponse<ICollection<ThemesResponse>> GetThemes(int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder)
        {
            var query = _repository.GetThemes().AsQueryable();

            query = query.Where(us => us.Active == true);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(us => us.Name.ToLower().Contains(search.ToLower()));
            }

            query = sortColumn switch
            {
                "Name" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Name) : query.OrderBy(us => us.Name),
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                _ => query.OrderBy(us => us.Id)
            };

            int totalItems = query.Count();

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            var result = query.ToList();

            var response = _mapper.Map<ICollection<ThemesResponse>>(result);

            return result.Any() ? ApiResponse<ICollection<ThemesResponse>>
            .Success(response, page, pageSize, totalItems)
            : ApiResponse<ICollection<ThemesResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<ThemesResponse> GetThemesById(long id)
        {
            var themes = _repository.GetThemesById(id);
            return (themes != null && (themes.Active == true))
                ? ApiResponse<ThemesResponse>.Success(_mapper.Map<ThemesResponse>(themes))
                : ApiResponse<ThemesResponse>.NotFound($"Không tìm thấy chủ đề #{id}");
        }

        public ApiResponse<ThemesResponse> CreateThemes(ThemesRequest themesRequest)
        {
            var existing = _repository.GetThemes().FirstOrDefault(us => us.Name?.ToLower() == themesRequest.Name.ToLower());
            if (existing != null)
            {
                return ApiResponse<ThemesResponse>.Conflict("Tên chủ đề đã tồn tại");
            }

            var created = _repository.CreateThemes(new Theme() { Name = themesRequest.Name, Active = true });
            return ApiResponse<ThemesResponse>.Success(_mapper.Map<ThemesResponse>(created));
        }

        public ApiResponse<ThemesResponse> UpdateThemes(long id, ThemesRequest themesRequest)
        {
            var existingTheme = _repository.GetThemesById(id);
            if (existingTheme == null || existingTheme.Active == false)
            {
                return ApiResponse<ThemesResponse>.NotFound("Không tìm thấy chủ đề.");
            }

            var existing = _repository.GetThemes()
                .FirstOrDefault(us => us.Id != id && us.Name?.ToLower() == themesRequest.Name.ToLower());
            if (existing != null)
            {
                return ApiResponse<ThemesResponse>.Conflict("Tên chủ đề đã tồn tại");
            }

            existingTheme.Name = themesRequest.Name;
            //existingTheme.Active = false;
            _repository.UpdateThemes(existingTheme);
            return ApiResponse<ThemesResponse>.Success(_mapper.Map<ThemesResponse>(existingTheme));
        }

        public ApiResponse<Theme> DeleteThemes(long id)
        {
            var existingTheme = _repository.GetThemesById(id);
            if (existingTheme == null)
            {
                return ApiResponse<Theme>.NotFound("Không tìm thấy chủ đề.");
            }

            if (existingTheme.Active == false)
            {
                return ApiResponse<Theme>.Conflict("chủ đề không tồn tại.");
            }

            existingTheme.Active = false;
            _repository.DeleteThemes(existingTheme);

            return ApiResponse<Theme>.Success();
        }

    }
}
