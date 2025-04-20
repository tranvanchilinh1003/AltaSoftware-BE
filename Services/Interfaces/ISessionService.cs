using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
//using System.collections.Generusing ISC_ELIB_SERVER.Services.Interfaces;
namespace ISC_ELIB_SERVER.Services.Interfaces;

public interface ISessionService
{
    ApiResponse<SessionResponse> GetSessionById(int id);
    ApiResponse<SessionResponse> CreateSession(SessionRequest request);
    ApiResponse<SessionResponse> UpdateSession(int id, SessionRequest request);
    ApiResponse<string> DeleteSession(int id);

    /// <summary>
    /// Allows a user to join an existing session.
    /// </summary>
    /// <param name="request">The request object containing the details needed to join the session.</param>
    /// <returns>An ApiResponse object containing the session response details.</returns>
    ApiResponse<SessionResponse> JoinSession(JoinSessionRequest request);

    ApiResponse<ICollection<SessionStudentResponse>> GetFilteredSessions(int userId, int page, int pageSize, SessionStudentFilterRequest request);

    ApiResponse<ICollection<SessionResponse>> GetSessions(int page, int pageSize, string search, string sortColumn, string sortOrder);

    ApiResponse<ICollection<TeacherDto>> GetTeachersBySubjectGroup(int UserId);

    ApiResponse<ICollection<ClassDto>> GetClassByTeacher(int UserId);

    // ApiResponse<ICollection<SessionStudentResponse>> GetFilteredSessions(SessionStudentFilterRequest filter);
   

    // <summary>
    /// Creates a new session for a teacher = teacharAssg .  
    ApiResponse<SessionResponse> CreateSession(int teacher_ID, SessionRequestTeacher request);


}



