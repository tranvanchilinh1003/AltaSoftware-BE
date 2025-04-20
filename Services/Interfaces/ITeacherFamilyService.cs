using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using AutoMapper;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Services
{
    public interface ITeacherFamilyService
    {
        ApiResponse<ICollection<TeacherFamilyResponse>> GetTeacherFamilies();
        ApiResponse<TeacherFamilyResponse> GetTeacherFamilyById(long id);
        ApiResponse<TeacherFamilyResponse> CreateTeacherFamily(TeacherFamilyRequest request);
        ApiResponse<TeacherFamilyResponse> UpdateTeacherFamily(long id, TeacherFamilyRequest request);
        ApiResponse<object> DeleteTeacherFamily(long id);
    }

    
}
