using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;

namespace ISC_ELIB_SERVER.Services
{

    public class SubjectGroupService : ISubjectGroupService
    {
        private readonly SubjectGroupRepo _subjectGroupRepo;
        private readonly IMapper _mapper;
        private readonly isc_dbContext _context;
        public SubjectGroupService(SubjectGroupRepo subjectGroupRepo, IMapper mapper, isc_dbContext context)
        {
            _subjectGroupRepo = subjectGroupRepo;
            _mapper = mapper;
            _context = context;
        }

        public ApiResponse<ICollection<SubjectGroupResponse>> GetSubjectGroup(int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder)
        {
            var query = _subjectGroupRepo.GetAllSubjectGroup().AsQueryable();

            query = query.Where(qr => qr.Active);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(us => us.Name.ToLower().Contains(search.ToLower()));
            }

            query = sortColumn?.ToLower() switch
            {
                "name" => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(us => us.Name) : query.OrderBy(us => us.Name),
                "id" => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                _ => query.OrderBy(us => us.Id)
            };
            query = query.Where(qr => qr.Active == true);

            var total = query.Count();

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            var result = query.ToList();

            var response = _mapper.Map<ICollection<SubjectGroupResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<SubjectGroupResponse>>.Success(
                        data: response,
                        totalItems: total,
                        pageSize: pageSize,
                        page: page
                    )
                : ApiResponse<ICollection<SubjectGroupResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<SubjectGroupResponse> GetSubjectGroupById(long id)
        {
            var subjectGroup = _subjectGroupRepo.GetSubjectGroupById(id);
            return subjectGroup != null
                ? ApiResponse<SubjectGroupResponse>.Success(_mapper.Map<SubjectGroupResponse>(subjectGroup))
                : ApiResponse<SubjectGroupResponse>.NotFound($"Không tìm thấy tổ - bộ môn có id {id}");
        }

        public ApiResponse<SubjectGroupResponse> CreateSubjectGroup(SubjectGroupRequest request)
        {
           

            var existing = _subjectGroupRepo.GetAllSubjectGroup().FirstOrDefault(st => st.Name?.ToLower() == request.Name.ToLower());
            if (existing != null)
            {
                return ApiResponse<SubjectGroupResponse>.Conflict("Tên tổ - bộ môn đã tồn tại");
            }
            var teacher = _context.Users.Where(u => u.Role.Name.ToLower() == "teacher").FirstOrDefault(x => x.Id == request.TeacherId);
            if (teacher == null)
            {
                return ApiResponse<SubjectGroupResponse>.NotFound($"Teacher có id {request.TeacherId} không tồn tại");
            }
            var create = _subjectGroupRepo.CreateSubjectGroup(_mapper.Map<SubjectGroup>(request));

            return ApiResponse<SubjectGroupResponse>.Success(_mapper.Map<SubjectGroupResponse>(create));
        }

        public ApiResponse<SubjectGroupResponse> UpdateSubjectGroup(long id, SubjectGroupRequest request)
        {
            var subjectGroup = _subjectGroupRepo.GetSubjectGroupById(id);
            if (subjectGroup == null)
            {
                return ApiResponse<SubjectGroupResponse>.NotFound($"Không tìm thấy tổ - bộ môn có id {id}");
            }
            var teacher = _context.Users.Where(u => u.Role.Name.ToLower() == "teacher").FirstOrDefault(x => x.Id == request.TeacherId);
            if (teacher == null)
            {
                return ApiResponse<SubjectGroupResponse>.NotFound($"Teacher có id {request.TeacherId} không tồn tại");
            }
            _mapper.Map(request, subjectGroup);
            var update = _subjectGroupRepo.UpdateSubjectGroup(subjectGroup);

            return ApiResponse<SubjectGroupResponse>.Success(_mapper.Map<SubjectGroupResponse>(update));
        }

        public ApiResponse<string> DeleteSubjectGroup(long id)
        {
            var delete = _subjectGroupRepo.DeleteSubjectGroup(id);
            if (delete)
            {
                return new ApiResponse<string>(0, "Xóa tổ - bộ môn thành công", null, null);
            }
            else
            {
                return ApiResponse<string>.NotFound($"Không tìm thấy tổ - bộ môn có id {id}");
            }
        }
    }
}
