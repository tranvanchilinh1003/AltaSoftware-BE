using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class SemesterRepo
    {
        private readonly isc_dbContext _context;
        public SemesterRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<Semester> GetSemesters()
        {
            return _context.Semesters
                .Where(a => a.Active)
                .ToList();
        }

        //public ICollection<object> GetScoreBySemesters(long userId, long academicYearId)
        //{
        //    var semesterScores = _context.Semesters
        //        .Where(s => s.AcademicYearId == academicYearId)
        //        .Select(s => new
        //        {
        //            Semester = s.Name,
        //            Scores = _context.StudentScores
        //                        .Where(ss => ss.SemesterId == s.Id && ss.UserId == userId)
        //                        .Select(ss => (decimal?)ss.Score)
        //                        .ToList()
        //        })
        //        .AsEnumerable()
        //        .Select(g => new
        //        {
        //            Semester = g.Semester,
        //            AverageScore = g.Scores.Any() ? Math.Round((decimal)g.Scores.Average()!, 1) : 0,
        //            Ranking = g.Scores.Any()
        //                ? ((decimal)g.Scores.Average()! >= (decimal)8 ? "Giỏi" :
        //                   (decimal)g.Scores.Average()! >= (decimal)6.5 ? "Khá" :
        //                   (decimal)g.Scores.Average()! >= (decimal)5 ? "Trung bình" : "Yếu")
        //                : "Chưa có điểm"
        //        })
        //        .OrderBy(x => x.Semester) 
        //        .ToList();

        //    if (semesterScores.Any())
        //    {
        //        var allScores = semesterScores.Where(x => x.AverageScore > 0).Select(x => x.AverageScore).ToList();
        //        decimal fullYearAverage = allScores.Any() ? Math.Round(allScores.Average(), 1) : 0;
        //        string fullYearRanking = allScores.Any()
        //            ? (fullYearAverage >= (decimal)8 ? "Giỏi" :
        //               fullYearAverage >= (decimal)6.5 ? "Khá" :
        //               fullYearAverage >= (decimal)5 ? "Trung bình" : "Yếu")
        //            : "Chưa có điểm";

        //        semesterScores.Add(new
        //        {
        //            Semester = "Cả năm",
        //            AverageScore = fullYearAverage,
        //            Ranking = fullYearRanking
        //        });
        //    }

        //    return semesterScores.Cast<object>().ToList();
        //}

        public ICollection<object> GetScoreBySemesters(long userId, long academicYearId)
        {
            var semesterScores = _context.Semesters
                .Where(s => s.AcademicYearId == academicYearId && s.Active)
                .Select(s => new
                {
                    Semester = s.Name,
                    Scores = _context.StudentScores
                                .Where(ss => ss.SemesterId == s.Id && ss.UserId == userId)
                                .Select(ss => (decimal?)ss.Score)
                                .ToList()
                })
                .AsEnumerable()
                .Select(g =>
                {
                    decimal average = g.Scores.Any() ? Math.Round((decimal)g.Scores.Average()!, 1) : 0;
                    string ranking = g.Scores.Any()
                        ? (average >= 8 ? "Giỏi"
                           : average >= (decimal)6.5 ? "Khá"
                           : average >= 5 ? "Trung bình"
                           : "Yếu")
                        : "Chưa có điểm";

                    string conduct = ranking; 

                    return new
                    {
                        Semester = g.Semester,
                        AverageScore = average,
                        Ranking = ranking,
                        Conduct = conduct
                    };
                })
                .OrderBy(x => x.Semester)
                .ToList();

            if (semesterScores.Any())
            {
                var allScores = semesterScores
                    .Where(x => x.AverageScore > 0)
                    .Select(x => x.AverageScore)
                    .ToList();

                decimal fullYearAverage = allScores.Any() ? Math.Round(allScores.Average(), 1) : 0;
                string fullYearRanking = allScores.Any()
                    ? (fullYearAverage >= 8 ? "Tốt"
                       : fullYearAverage >= (decimal)6.5 ? "Khá"
                       : fullYearAverage >= 5 ? "Trung bình"
                       : "Yếu")
                    : "Chưa có điểm";

                string fullYearConduct = fullYearRanking;

                semesterScores.Add(new
                {
                    Semester = "Cả năm",
                    AverageScore = fullYearAverage,
                    Ranking = fullYearRanking,
                    Conduct = fullYearConduct
                });
            }

            return semesterScores.Cast<object>().ToList();
        }



        public ICollection<object> GetStudentScores(long userId, long academicYearId)
        {
            var rawData = from s in _context.Semesters
                          join ay in _context.AcademicYears on s.AcademicYearId equals ay.Id
                          join ss in _context.StudentScores on s.Id equals ss.SemesterId
                          join sub in _context.Subjects on ss.SubjectId equals sub.Id
                          join st in _context.ScoreTypes on ss.ScoreTypeId equals st.Id
                          where ss.UserId == userId && ay.Id == academicYearId
                          select new
                          {
                              SemesterId = s.Id,
                              Semester = s.Name,
                              SubjectId = sub.Id,
                              Subject = sub.Name,
                              ScoreType = st.Name,
                              Score = ss.Score
                          };

            var result = rawData
                .GroupBy(x => new { x.SemesterId, x.Semester })
                .Select(g => new
                {
                    Semester = g.Key.Semester,
                    Subjects = g.GroupBy(x => new { x.SubjectId, x.Subject })
                                .Select(sg => new
                                {
                                    Subject = sg.Key.Subject,
                                    Scores = sg.Select(x => new { x.ScoreType, x.Score }).ToList(),
                                    Average = sg.Any() ? Math.Round((decimal)sg.Average(x => x.Score), 1) : (decimal?)null
                                }).ToList()
                }).ToList<object>();

            var allSemesters = _context.Semesters
                .Where(s => s.AcademicYearId == academicYearId && s.Active)
                .Select(s => new
                {
                    SemesterId = s.Id,
                    Semester = s.Name
                }).ToList();

            var finalResult = allSemesters.Select(s => new
            {
                Semester = s.Semester,
                Subjects = result.Cast<dynamic>().FirstOrDefault(r => r.Semester == s.Semester)?.Subjects ??
                           new List<object> { new { Subject = "Chưa có điểm", Scores = new List<object>(), Average = (decimal?)null } }
            }).ToList<object>();

            return finalResult;
        }



        public ICollection<object> GetCourseOfSemesters(int userId)
        {
            var dayOfWeekMapping = new Dictionary<DayOfWeek, string>
            {
                { DayOfWeek.Sunday, "Chủ Nhật" },
                { DayOfWeek.Monday, "Thứ 2" },
                { DayOfWeek.Tuesday, "Thứ 3" },
                { DayOfWeek.Wednesday, "Thứ 4" },
                { DayOfWeek.Thursday, "Thứ 5" },
                { DayOfWeek.Friday, "Thứ 6" },
                { DayOfWeek.Saturday, "Thứ 7" }
            }                                   ;

            var statusMapping = new Dictionary<string, string>
            {
                { "Scheduled", "Chưa hoàn thành" },
                { "Ongoing", "Đang diễn ra" },
                { "Completed", "Đã hoàn thành" }
            };

            var rawData = (from ta in _context.TeachingAssignments
                           where ta.UserId == userId
                           join s in _context.Semesters on ta.SemesterId equals s.Id
                           join ay in _context.AcademicYears on s.AcademicYearId equals ay.Id
                           join c in _context.Classes on ta.ClassId equals c.Id
                           join sub in _context.Subjects on ta.SubjectId equals sub.Id
                           join ses in _context.Sessions on ta.Id equals ses.TeachingAssignmentId into sesGroup
                           from ses in sesGroup.DefaultIfEmpty()

                           select new
                           {
                               SemesterId = s.Id,
                               Semester = s.Name,
                               Subject = sub.Name,
                               Class = c.Name,
                               StartDate = ta.StartDate,
                               EndDate = ta.EndDate,
                               SessionStart = ses != null ? ses.StartDate : null,
                               Status = ses != null ? ses.Status : null,
                               TeachingAssignmentId = ta.Id
                           }).ToList();

            var groupedSessions = rawData
                .GroupBy(x => x.TeachingAssignmentId)
                .Select(g => new
                {
                    TeachingAssignmentId = g.Key,
                    EarliestSession = g.Where(x => x.SessionStart.HasValue)
                                       .OrderBy(x => x.SessionStart)
                                       .FirstOrDefault()
                }).ToDictionary(x => x.TeachingAssignmentId, x => x.EarliestSession);

            var transformedData = rawData
                .GroupBy(x => new { x.SemesterId, x.Semester, x.Subject, x.Class, x.StartDate, x.EndDate })
                .Select(g => new
                {
                    g.Key.SemesterId,
                    g.Key.Semester,
                    Subject = g.Key.Subject,
                    Class = g.Key.Class,
                    Schedule = groupedSessions.ContainsKey(g.First().TeachingAssignmentId) &&
                               groupedSessions[g.First().TeachingAssignmentId]?.SessionStart != null
                        ? dayOfWeekMapping[groupedSessions[g.First().TeachingAssignmentId].SessionStart.Value.DayOfWeek] +
                          " - " + groupedSessions[g.First().TeachingAssignmentId].SessionStart.Value.ToString("HH:mm")
                        : "Không có lịch",
                    Date = $"{g.Key.StartDate?.ToString("dd/MM") ?? ""} - {g.Key.EndDate?.ToString("dd/MM") ?? ""}",
                    Status = g.First().Status != null && statusMapping.ContainsKey(g.First().Status)
                        ? statusMapping[g.First().Status]
                        : "Không xác định"
                }).ToList();

            var groupedData = transformedData
                .GroupBy(x => new { x.SemesterId, x.Semester })
                .Select(g => new
                {
                    Id = g.Key.SemesterId,
                    Semester = g.Key.Semester,
                    Courses = g.Select(x => new
                    {
                        x.Subject,
                        x.Class,
                        x.Schedule,
                        x.Date,
                        x.Status
                    }).ToList()
                }).ToList<object>();

            return groupedData;
        }








        public Semester GetSemesterById(long id)
        {
            return _context.Semesters.Where(a => a.Active).FirstOrDefault(s => s.Id == id);
        }

        public Semester CreateSemester(Semester Semester)
        {
            _context.Semesters.Add(Semester);
            _context.SaveChanges();
            return Semester;
        }

        public Semester UpdateSemester(Semester Semester)
        {
            _context.Semesters.Update(Semester);
            _context.SaveChanges();
            return Semester;
        }

        public bool DeleteSemester(long id)
        {
            var Semester = GetSemesterById(id);
            if (Semester != null)
            {
                Semester.Active = false;
                _context.Semesters.Update(Semester);
                return _context.SaveChanges() > 0;
            }
            return false;
        }


        // Lành -- Cần học kỳ theo niên khoá
        public ICollection<Semester> GetSemestersByAcademicYearId(long academicYearId)
        {
            return _context.Semesters.Where(s => s.Active && s.AcademicYearId == academicYearId).ToList();
        }
    }
}