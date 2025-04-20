using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;
using Sprache;

namespace ISC_ELIB_SERVER.Services
{

    public class ResignationService : IResignationService
    {
        private readonly ResignationRepo _repository;
        private readonly IMapper _mapper;
        private readonly isc_dbContext _context;

        public ResignationService(ResignationRepo repository, IMapper mapper, isc_dbContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
        }

        public ApiResponse<ResignationResponse> CreateResignation(ResignationRequest Resignation_AddRequest)
        {
            if (Resignation_AddRequest.Date.HasValue)
            {
                Resignation_AddRequest.Date = DateTime.SpecifyKind(Resignation_AddRequest.Date.Value, DateTimeKind.Unspecified);
            }
            var Resignation = _mapper.Map<Resignation>(Resignation_AddRequest);
            var created = _repository.CreateResignation(Resignation);
            return ApiResponse<ResignationResponse>.Success(_mapper.Map<ResignationResponse>(created));
        }

        public ApiResponse<Resignation> DeleteResignation(long id)
        {
            var success = _repository.DeleteResignation(id);
            return success
                ? ApiResponse<Resignation>.Success()
                : ApiResponse<Resignation>.NotFound("Không tìm thấy dữ liệu để xóa");
        }

        public ApiResponse<ICollection<ResignationResponse>> GetResignation(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetResignation().AsQueryable();


            query = sortColumn switch
            {
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                _ => query.OrderBy(us => us.Id)
            };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var response = _mapper.Map<ICollection<ResignationResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<ResignationResponse>>.Success(response)
                : ApiResponse<ICollection<ResignationResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<ICollection<ResignationResponse>> GetResignationNoPaging()
        {
            var exsting = _repository.GetResignation();
            var response = _mapper.Map<ICollection<ResignationResponse>>(exsting);
            return exsting.Any()
                ? ApiResponse<ICollection<ResignationResponse>>.Success(response)
                : ApiResponse<ICollection<ResignationResponse>>.NotFound("Không có dữ liệu");
        }


        public ApiResponse<ResignationResponse> GetResignationById(long id)
        {
            var Resignation = _repository.GetResignationById(id);
            return Resignation != null
                ? ApiResponse<ResignationResponse>.Success(_mapper.Map<ResignationResponse>(Resignation))
                : ApiResponse<ResignationResponse>.NotFound($"Không tìm thấy trạng từ chức #{id}");
        }

        public ApiResponse<ICollection<ResignationResponse>> GetResignationByTeacherId(long id)
        {

            var resignations = _repository.GetResignationsByTeacherId(id);

            if (resignations == null || !resignations.Any())
            {
                return ApiResponse<ICollection<ResignationResponse>>.NotFound("Không có dữ liệu");
            }

            var response = resignations.Select(r => new ResignationResponse
            {
                Id = r.Id,
                TeacherId = (int)r.TeacherId,
                Date = r.Date,
                Note = r.Note,
                Attachment = r.Attachment,
                Status = r.Status
            }).ToList();

            return ApiResponse<ICollection<ResignationResponse>>.Success(response);
        }

        public ApiResponse<Resignation> UpdateResignation(long id, ResignationRequest ResignationRequest)
        {
            if (ResignationRequest.Date.HasValue)
            {
                // Chuyển sang DateTime có Kind Unspecified
                ResignationRequest.Date = DateTime.SpecifyKind(ResignationRequest.Date.Value, DateTimeKind.Unspecified);
            }
            var Resignation = _mapper.Map<Resignation>(ResignationRequest);
            var updated = _repository.UpdateResignation(id, ResignationRequest);
            return updated != null
                ? ApiResponse<Resignation>.Success(updated)
                : ApiResponse<Resignation>.NotFound("Không tìm thấy trạng thái từ chức để cập nhật");
        }

        

    
    }
}
