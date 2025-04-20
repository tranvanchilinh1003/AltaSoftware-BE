using AutoMapper;
using CloudinaryDotNet;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace ISC_ELIB_SERVER.Services
{
    public class ClassTypeService : IClassTypeService
    {
        private readonly IClassTypeRepo _repository;
        private readonly ClassRepo _classRepository;
        private readonly AcademicYearRepo _academicYearRepository;
        private readonly IMapper _mapper;

        public ClassTypeService(IClassTypeRepo repository, ClassRepo classRepository, AcademicYearRepo academicYearRepository, IMapper mapper)
        {
            _repository = repository;
            _classRepository = classRepository;
            _academicYearRepository = academicYearRepository;
            _mapper = mapper;
        }


        public ApiResponse<ClassTypeResponse> GetClassTypeById(int id)
        {
            var classType = _repository.GetClassTypeById(id);

            if (classType == null || !classType.Active)
            {
                return ApiResponse<ClassTypeResponse>.NotFound("Không tìm thấy loại lớp");
            }

            var response = _mapper.Map<ClassTypeResponse>(classType);
            return ApiResponse<ClassTypeResponse>.Success(response);
        }


        public ApiResponse<ClassTypeResponse> CreateClassType(ClassTypeRequest classTypeRequest)
        {
            if (string.IsNullOrWhiteSpace(classTypeRequest.Name))
            {
                return ApiResponse<ClassTypeResponse>.BadRequest("Tên loại lớp không được để trống");
            }

            bool isDuplicate = _repository.GetClassTypes()
                .Any(ct => ct.Name.ToLower() == classTypeRequest.Name.ToLower());

            if (isDuplicate)
            {
                return ApiResponse<ClassTypeResponse>.Conflict("Tên loại lớp đã tồn tại");
            }

            bool academicYearExists = _academicYearRepository.GetAcademicYears()
                .Any(ay => ay.Id == classTypeRequest.AcademicYearId);

            if (!academicYearExists)
            {
                return ApiResponse<ClassTypeResponse>.BadRequest("Niên khóa không hợp lệ");
            }

            var classType = _mapper.Map<ClassType>(classTypeRequest);
            classType.AcademicYearId = (int)classTypeRequest.AcademicYearId; 
            classType.Active = false;
            _repository.CreateClassType(classType);

            return ApiResponse<ClassTypeResponse>.Success(_mapper.Map<ClassTypeResponse>(classType));
        }


        public ApiResponse<ClassTypeResponse> UpdateClassType(int id, ClassTypeRequest classTypeRequest)
        {
            if (string.IsNullOrWhiteSpace(classTypeRequest.Name))
            {
                return ApiResponse<ClassTypeResponse>.BadRequest("Tên loại lớp không được để trống");
            }

            var existingClassType = _repository.GetClassTypeById(id);
            if (existingClassType == null)
            {
                return ApiResponse<ClassTypeResponse>.NotFound("Không tìm thấy loại lớp");
            }

            bool isDuplicate = _repository.GetClassTypes()
                .Any(ct => ct.Name.ToLower() == classTypeRequest.Name.ToLower() && ct.Id != id);

            if (isDuplicate)
            {
                return ApiResponse<ClassTypeResponse>.Conflict("Tên loại lớp đã tồn tại");
            }

            if (classTypeRequest.AcademicYearId != existingClassType.AcademicYearId)
            {
                bool academicYearExists = _academicYearRepository.GetAcademicYears()
                    .Any(ay => ay.Id == classTypeRequest.AcademicYearId);

                if (!academicYearExists)
                {
                    return ApiResponse<ClassTypeResponse>.BadRequest("Niên khóa không hợp lệ");
                }
            }

            _mapper.Map(classTypeRequest, existingClassType);


            var updatedClassType = _repository.UpdateClassType(existingClassType);

            return ApiResponse<ClassTypeResponse>.Success(_mapper.Map<ClassTypeResponse>(updatedClassType));
        }



        public ApiResponse<bool> DeleteClassType(int id)
        {
            var deleted = _repository.DeleteClassType(id);
            return deleted
                ? ApiResponse<bool>.Success(true)
                : ApiResponse<bool>.NotFound("Không tìm thấy loại lớp để xóa");
        }

        public ApiResponse<ICollection<ClassTypeResponse>> GetClassTypes(
     int? page, int? pageSize, int? searchYear, string? searchName, string? sortColumn, string? sortOrder)
        {
            if (!searchYear.HasValue)
            {
                return ApiResponse<ICollection<ClassTypeResponse>>.BadRequest("Niên khóa không được để trống");
            }

            var academicYearExists = _academicYearRepository.GetAcademicYears()
                .Any(ay => ay.Id == searchYear.Value);

            if (!academicYearExists)
            {
                return ApiResponse<ICollection<ClassTypeResponse>>.NotFound("Niên khóa không tồn tại");
            }

            var query = _repository.GetClassTypes()
                .Where(ct => ct.AcademicYearId == searchYear.Value && ct.Active);

            if (!string.IsNullOrWhiteSpace(searchName))
            {
                query = query.Where(ct => ct.Name.ToLower().Contains(searchName.ToLower()));
            }

            query = sortColumn?.ToLower() switch
            {
                "id" => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(ct => ct.Id) : query.OrderBy(ct => ct.Id),
                _ => query.OrderBy(ct => ct.Id)
            };

            int totalRecords = query.Count();

            if (page.HasValue && pageSize.HasValue && page > 0 && pageSize > 0)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            var result = query.ToList();
            var response = _mapper.Map<ICollection<ClassTypeResponse>>(result);

            return response.Any()
                ? ApiResponse<ICollection<ClassTypeResponse>>.Success(response, page, pageSize, totalRecords)
                : ApiResponse<ICollection<ClassTypeResponse>>.NotFound("Không có dữ liệu");
        }
    }
}
