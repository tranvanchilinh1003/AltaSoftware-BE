using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;

namespace ISC_ELIB_SERVER.Services
{
    public class EntryTypeService : IEntryTypeService
    {
        private readonly EntryTypeRepo _repository;
        private readonly IMapper _mapper;

        public EntryTypeService(EntryTypeRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<EntryTypeResponse>> GetEntryTypes(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetEntryTypes().AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(et => et.Name.ToLower().Contains(search.ToLower()));
            }

            query = sortColumn switch
            {
                "Name" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(et => et.Name) : query.OrderBy(et => et.Name),
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(et => et.Id) : query.OrderBy(et => et.Id),
                _ => query.OrderBy(et => et.Id)
            };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var response = _mapper.Map<ICollection<EntryTypeResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<EntryTypeResponse>>.Success(response)
                : ApiResponse<ICollection<EntryTypeResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<EntryTypeResponse> GetEntryTypeById(long id)
        {
            var entryType = _repository.GetEntryTypeById(id);
            return entryType != null
                ? ApiResponse<EntryTypeResponse>.Success(_mapper.Map<EntryTypeResponse>(entryType))
                : ApiResponse<EntryTypeResponse>.NotFound($"Không tìm thấy loại đầu vào #{id}");
        }

        public ApiResponse<EntryTypeResponse> CreateEntryType(EntryTypeRequest entryTypeRequest)
        {
            var existing = _repository.GetEntryTypes()
            .FirstOrDefault(et => et.Name?.ToLower() == entryTypeRequest.Name.ToLower());
            if (existing != null)
            {
                return ApiResponse<EntryTypeResponse>.Conflict("Tên loại đầu vào đã tồn tại");
            }

            var entryType = _mapper.Map<EntryType>(entryTypeRequest);

            try
            {
                var created = _repository.CreateEntryType(entryType);
                return ApiResponse<EntryTypeResponse>.Success(_mapper.Map<EntryTypeResponse>(created));
            }
            catch (Exception)
            {
                return ApiResponse<EntryTypeResponse>.BadRequest("Dữ liệu đầu vào không hợp lệ");
            }
        }

        public ApiResponse<EntryTypeResponse> UpdateEntryType(long id, EntryTypeRequest entryTypeRequest)
        {
            var entryType = _repository.GetEntryTypeById(id);
            if (entryType == null)
            {
                return ApiResponse<EntryTypeResponse>.NotFound($"Không tìm thấy loại đầu vào #{id}");
            }

            entryType.Name = entryTypeRequest.Name;
            var updated = _repository.UpdateEntryType(entryType);

            return ApiResponse<EntryTypeResponse>.Success(_mapper.Map<EntryTypeResponse>(updated));
        }

        public ApiResponse<object> DeleteEntryType(long id)
        {
            var entryType = _repository.GetEntryTypeById(id);
            if (entryType == null)
            {
                return ApiResponse<object>.NotFound("Không tìm thấy loại đầu vào để xóa");
            }

            entryType.Active = false; // Xóa mềm
            _repository.UpdateEntryType(entryType);
            return ApiResponse<object>.Success("Xóa thành công");
        }
    }
}
