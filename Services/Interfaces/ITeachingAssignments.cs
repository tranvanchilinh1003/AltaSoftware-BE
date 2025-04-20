using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface ITeachingAssignmentsService
    {
        ApiResponse<ICollection<TeachingAssignmentsResponse>> GetTeachingAssignmentsNotExpired(
            int? page, int? pageSize, string? sortColumn, string? sortOrder,string? searchSubject,int? subjectId,int? subjectGroupId);
        ApiResponse<ICollection<TeachingAssignmentsResponse>> GetTeachingAssignmentsExpired(
           int? page, int? pageSize, string? sortColumn, string? sortOrder, string? searchSubject, int? subjectId, int? subjectGroupId);

        ApiResponse<ICollection<TeachingAssignmentsResponse>> GetTeacherByAcademicYearAndSubjectGroup(int? academicYearId, int? subjectGroupId,
        int? page, int? pageSize, string? sortColumn, string? sortOrder, string? search);

        ApiResponse<ICollection<TeachingAssignmentsResponse>> GetTeachingAssignmentsByTeacher(int? page, int? pageSize, string? sortColumn, string? sortOrder, int? teacherId);
        ApiResponse<TeachingAssignmentsResponse> GetTeachingAssignmentById(int id);
        ApiResponse<TeachingAssignmentsResponse> CreateTeachingAssignment(TeachingAssignmentsRequest teachingAssignmentRequest);
        ApiResponse<TeachingAssignmentsResponse> UpdateTeachingAssignment(int id, TeachingAssignmentsRequest teachingAssignmentRequest);
        ApiResponse<bool> DeleteTeachingAssignment(List<int> ids);

        ApiResponse<bool> UpdateTimeTeachingAssignment(int id);


    }
}
