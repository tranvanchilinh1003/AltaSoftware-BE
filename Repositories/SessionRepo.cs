using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using System.Collections.Generic;
using ISC_ELIB_SERVER.Utils;
using System.Linq;

using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class SessionRepo
    {
        private readonly isc_dbContext _context;
        public SessionRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<Session> GetAllSessions()
        {
            return _context.Sessions.ToList();
        }

        public int Count()
        {
            return _context.Sessions.Count();
        }

        public Session GetSessionById(int id)
        {
            return _context.Sessions.FirstOrDefault(s => s.Id == id);
        }

        public Session CreateSession(Session session)
        {


            _context.Sessions.Add(session);
            _context.SaveChanges();
            return session;
        }

        public bool DeleteSession(int id)
        {
            var session = GetSessionById(id);
            if (session != null)
            {
                _context.Sessions.Remove(session);
                return _context.SaveChanges() > 0;
            }
            return false;
        }

        public Session UpdateSession(Session session)
        {
            _context.Sessions.Update(session);
            _context.SaveChanges();
            return session;
        }


        public Session? GetSessionByJoinInfo(JoinSessionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ShareCodeOrSessionId) || string.IsNullOrWhiteSpace(request.Password))
            {
                return null; // Không có đủ thông tin để tìm lớp
            }

            var query = _context.Sessions.AsQueryable();
            Session? session = null;

            // Kiểm tra nếu nhập số thì tìm theo SessionId
            if (int.TryParse(request.ShareCodeOrSessionId, out int sessionId))
            {
                session = query.FirstOrDefault(s => s.Id == sessionId);
            }
            else
            {
                // Nếu không phải số, tìm theo ShareCodeUrl
                session = query.FirstOrDefault(s => s.ShareCodeUrl == request.ShareCodeOrSessionId);
            }

            // Kiểm tra mật khẩu
            string hashedInputPassword = PasswordHasher.HashPassword(request.Password);
            if (session != null && session.Password == hashedInputPassword)
            {
                return session; // Trả về session nếu tìm thấy và mật khẩu đúng
            }

            return null; // Trả về null nếu không tìm thấy hoặc sai mật khẩu
        }


        public ICollection<SessionStudentResponse> GetFilteredSessions(int userId, SessionStudentFilterRequest request)
        {
            // mội học sinh chỉ có thể ở một lớp trong khóa-khối không thể có trong nhiều lớp trong cùng một khóa-khối
            var query = _context.Sessions.AsQueryable();

            // Lấy tất cả lớp học của sinh viên
            var studentClasses = _context.ClassUsers
                                         .Where(cu => cu.UserId == userId && cu.Class != null)
                                        .Select(cu => cu.Class)
                                         .ToList();

            if (studentClasses == null || !studentClasses.Any())
            {
                return new List<SessionStudentResponse>(); // Sinh viên không có lớp nào
            }

            // Lọc các lớp học đang hoạt động
            var activeClassIds = studentClasses.Where(c => c.Active).Select(c => c.Id).ToList();

            if (!activeClassIds.Any())
            {
                return new List<SessionStudentResponse>(); // Không có lớp nào đang hoạt động
            }

            // Lấy tất cả TeachingAssignments thuộc các lớp này
            var teachingAssignments = _context.TeachingAssignments
                                              .Where(ta => activeClassIds.Contains(ta.Class.Id))
                                              .Select(ta => ta.Id);

            query = query.Where(s => s.TeachingAssignmentId.HasValue && teachingAssignments.Contains(s.TeachingAssignmentId.Value));

            // Lọc theo ngày
            if (request.Date.HasValue)
            {
                query = query.Where(s => s.StartDate.HasValue && s.StartDate.Value.Date == request.Date.Value.Date);
            }

            // Lọc theo môn học
            if (request.SubjectId.HasValue)
            {
                query = query.Where(s => s.TeachingAssignment != null && s.TeachingAssignment.SubjectId == request.SubjectId.Value);
            }

            // Lọc theo niên khóa
            if (request.AcademicYearId.HasValue)
            {
                query = query.Where(s => s.TeachingAssignment != null &&
                                         s.TeachingAssignment.Class != null &&
                                         s.TeachingAssignment.Class.AcademicYearId == request.AcademicYearId.Value);
            }

            // Lọc theo tên topic (tìm kiếm gần đúng)
            if (!string.IsNullOrEmpty(request.TopicName))
            {
                query = query.Where(s => EF.Functions.ILike(s.TeachingAssignment.Topics.Name, $"%{request.TopicName}%"));
            }

            // Lọc theo trạng thái lớp
            if (!string.IsNullOrEmpty(request.Status))
            {
                switch (request.Status.ToLower())
                {
                    case "chuabatdau":
                        query = query.Where(s => s.StartDate.HasValue && s.StartDate > DateTime.Now);
                        break;
                    case "dangdienra":
                        query = query.Where(s => s.StartDate.HasValue && s.EndDate.HasValue &&
                                                 s.StartDate <= DateTime.Now && s.EndDate >= DateTime.Now);
                        break;
                    case "dahoanthanh":
                        query = query.Where(s => s.EndDate.HasValue && s.EndDate < DateTime.Now);
                        break;
                }
            }

            // Sắp xếp dữ liệu
            query = request.SortOrder.ToLower() == "desc"
                ? query.OrderByDescending(s => request.SortColumn.ToLower() == "startdate"
                    ? (object)s.StartDate
                    : (object)s.TeachingAssignment.Subject.Name)
                : query.OrderBy(s => request.SortColumn.ToLower() == "startdate"
                    ? (object)s.StartDate
                    : (object)s.TeachingAssignment.Subject.Name);

            // Trả về danh sách kết quả
            return query.Select(s => new SessionStudentResponse
            {
                ClassId = s.TeachingAssignment.Class.Id,
                ClassCode = s.TeachingAssignment.Class.Code,
                SessionId = s.Id,
                Subject = new SubjectDto
                {
                    Id = s.TeachingAssignment.Subject.Id,
                    Name = s.TeachingAssignment.Subject.Name
                },
                Teacher = new TeacherDto
                {
                    Id = s.TeachingAssignment.User.Id,
                    Name = s.TeachingAssignment.User.FullName
                },
                Status = s.Status,
                SessionTime = s.StartDate.HasValue ? s.StartDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "N/A"
            }).ToList();
        }


        public bool IsValidExamId(int examId)
        {
            return _context.Exams.Any(e => e.Id == examId);
        }

        public bool IsValidTeachingAssignmentId(int teachingAssignmentId)
        {
            return _context.TeachingAssignments.Any(ta => ta.Id == teachingAssignmentId);
        }


        public ICollection<TeacherDto> GetTeachersBySubjectGroup(int UserId)
        {
            // Lấy danh sách giáo viên theo môn học và lớp học của người dùng
            var subject_groups = _context.Subjects
               .Where(s => s.Active && s.SubjectGroup.TeacherId == UserId)
               .Select(s => s.SubjectGroup);
            Console.WriteLine($"Subject Groups: {String.Join(",", subject_groups.Select(sg => sg.Name + " id subject_groups (" + sg.Id + ")" + " -teacher ID " + sg.TeacherId))}");
            Console.WriteLine($"UserId: {UserId}");
            var subject_groups_id = subject_groups.Select(sg => sg.Id).ToList();
            var tearcher = _context.SubjectGroups
                .Where(sg => subject_groups_id.Contains(sg.Id) && sg.Active)
                .SelectMany(sg => sg.Subjects.SelectMany(s => s.TeachingAssignments.Select(t => new
                {
                    Teacher = t.User,
                    Subject = s,
                    Class = t.Class
                })))
                .Distinct()
                .ToList();
            return tearcher.Select(t => new TeacherDto
            {
                Id = t.Teacher.Id,
                Name = t.Teacher.FullName,
                // SubjectId = t.Subject.Id,
                // SubjectName = t.Subject.Name,
                // ClassId = t.Class.Id,
                // ClassCode = t.Class.Code
            }).ToList();
            // var query = _context.Users
            //      .Where(u => u.Id == UserId && u.Active && u.TeachingAssignments.Any())
            //      .SelectMany(u => u.TeachingAssignments.Select(ta => new TeacherDto
            //      {
            //          Id = ta.User.Id,
            //          Name = ta.User.FullName,
            //          // SubjectId = ta.Subject.Id,
            //          // SubjectName = ta.Subject.Name,
            //          // ClassId = ta.Class.Id,
            //          // ClassCode = ta.Class.Code
            //      }))
            //      .ToList();

            // return query;
        }

        //   teacher_ID, class_ID,

        public List<TeachingAssignment>? GetTeachingAssignments(int teacherId, int? classId = null)
        {
            var teachingAssignments = _context.TeachingAssignments
            .Where(ta => ta.UserId == teacherId && (!classId.HasValue || ta.ClassId == classId))
            .ToList();

            return teachingAssignments.Any() ? teachingAssignments : null;
        }

        // public Session CreateSessionTeacher(int teacher_ID, SessionRequestTeacher request)
        // {
        //     if (request.ClassId != null)
        //     {
        //         var teaching_assignments1 = _context.TeachingAssignments
        //             .Where(ta => ta.UserId == teacher_ID && ta.ClassId == request.ClassId)
        //             .ToList();

        //         if (teaching_assignments1 == null || !teaching_assignments1.Any())
        //         {
        //             throw new Exception("No teaching assignments found for the given teacher and class.");
        //         }

        //         // Map all fields from SessionRequestTeacher to Session
        //         var session = new Session
        //         {
        //             Name = request.Name,
        //             StartDate = request.StartDate,
        //             EndDate = request.EndDate,
        //             Description = request.Description,
        //             TeachingAssignmentId = teaching_assignments1.First().Id,
        //             ShareCodeUrl = request.ShareCodeUrl,
        //             Password = request.Password,
        //             Status = request.Status
        //         };

        //         _context.Sessions.Add(session);
        //         _context.SaveChanges();
        //         return session;
        //     }

        //     var teaching_assignments = _context.TeachingAssignments
        //     .Where(ta => ta.UserId == teacher_ID)
        //     .ToList();

        //     if (teaching_assignments == null || !teaching_assignments.Any())
        //     {
        //         throw new Exception("No teaching assignments found for the given teacher.");
        //     }

        //     // Map all fields from SessionRequestTeacher to Session
        //     var sessionFallback = new Session
        //     {
        //         Name = request.Name,
        //         StartDate = request.StartDate,
        //         EndDate = request.EndDate,
        //         Description = request.Description,
        //         TeachingAssignmentId = teaching_assignments.First().Id,
        //         ShareCodeUrl = request.ShareCodeUrl,
        //         Password = request.Password,
        //         Status = request.Status
        //     };

        //     _context.Sessions.Add(sessionFallback);
        //     _context.SaveChanges();
        //     return sessionFallback;
        // }

        public ICollection<ClassDto> GetClassByTeacher(int teacherId)
        {
            var classDto = _context.Classes
            .Where(c => c.TeachingAssignments.Any(ta => ta.UserId == teacherId) && c.Active)
            .Select(c => new ClassDto
            {
                Id = c.Id,
                Name = c.Name,
                Code = c.Code,
                Active = c.Active
            })
            .ToList();

            return classDto;
        }

        public bool CheckSessionTimeConflict(DateTime startTime, DateTime endTime, int? ignoreSessionId = null)
        {
            // Ensure DateTime.Kind is Unspecified
            startTime = DateTime.SpecifyKind(startTime, DateTimeKind.Unspecified);
            endTime = DateTime.SpecifyKind(endTime, DateTimeKind.Unspecified);

            var sessions = _context.Sessions.AsQueryable();

            // Nếu đang cập nhật, loại trừ session hiện tại
            if (ignoreSessionId.HasValue)
            {
                sessions = sessions.Where(s => s.Id != ignoreSessionId.Value);
            }
            // 
            foreach (var session in sessions)
            {
                if (startTime <= session.EndDate)
                {
                    return false; //
                }
            }
            return true;

            // return sessions.Any(s =>
            //     s.StartDate.HasValue && s.EndDate.HasValue &&
            //     (
            //         // Bắt đầu mới nằm trong session cũ
            //         (s.StartDate.Value <= startTime && s.EndDate.Value > startTime) ||
            //         // Kết thúc mới nằm trong session cũ
            //         (s.StartDate.Value < endTime && s.EndDate.Value >= endTime) ||
            //         // Session mới bao trùm session cũ
            //         (s.StartDate.Value >= startTime && s.EndDate.Value <= endTime) ||
            //         // Session cũ bao trùm session mới
            //         (s.StartDate.Value <= startTime && s.EndDate.Value >= endTime)
            //     )
            // );
        }

        //   if (session.TeachingAssistantId.HasValue)
        // {
        //     var taExists = _context.Users.Any(u => u.Id == session.TeachingAssistantId.Value);
        //     if (!taExists)
        //     {
        //         return ApiResponse<SessionResponse>.BadRequest($"TeachingAssistantId } không hợp lệ.");
        //     }
        // }

        public bool TeacherAssistant(int TeachetAssistantId)
        {
            var teachAssistant = _context.Users.Where(u => u.Id == TeachetAssistantId).ToList();
            Console.WriteLine($"Teaching Assistant: {String.Join(",", teachAssistant.Select(ta => ta.FullName + " id " + ta.Id))}");
            if (teachAssistant.Count() == 0)
            {
                return false; // Không tìm thấy Teaching Assistant
            }
            return true;
        }






    }
}
