using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Enums;
using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class TestRepo
    {
        private readonly isc_dbContext _context;
        private readonly IMapper _mapper;
        public TestRepo(isc_dbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IQueryable<Test> GetTests()
        {
            return _context.Tests
                .Include(t => t.Subject)
                    .ThenInclude(s => s.SubjectGroup)
                .Include(t => t.Subject)
                    .ThenInclude(s => s.SubjectType)
                .Include(t => t.User)
                .Include(t => t.GradeLevel);
        }

        public IEnumerable<TestByStudentResponse> GetTestsByStudent(int userId)
        {
            return _context.TestUsers
                        .Where(tu => tu.UserId == userId && tu.Test.Active == true)
                        .Include(tu => tu.Test)
                            .ThenInclude(t => t.Subject)
                                 .ThenInclude(t => t.SubjectGroup)
                        .Include(tu => tu.Test)
                            .ThenInclude(t => t.Subject)
                                .ThenInclude(t => t.SubjectType)
                        .Include(tu => tu.Test)
                            .ThenInclude(t => t.User)
                        .Include(tu => tu.Test)
                            .ThenInclude(t => t.GradeLevel)
                        .AsEnumerable()
                        .Select(su => new TestByStudentResponse
                        {
                            Test = _mapper.Map<TestResponse>(su.Test),
                            User = _mapper.Map<UserResponse>(su.User),
                            Status = su.Status
                        });
        }
        public Test GetTestById(long id)
        {
            return _context.Tests.FirstOrDefault(s => s.Id == id);
        }

        public Test CreateTest(Test test)
        {
            _context.Tests.Add(test);
            _context.SaveChanges();
            return test;
        }

        public Test UpdateTest(Test test)
        {
            _context.Tests.Update(test);
            _context.SaveChanges();
            return test;
        }


        public bool DeleteTest(long id)
        {
            var test = GetTestById(id);
            if (test != null)
            {
                test.Active = false;
                _context.Tests.Update(test);
                return _context.SaveChanges() > 0;
            }
            return false;
        }

        // Thêm cho chức năng lấy bài kiểm tra them môn học
        public Test GetTestsBySubjectId(int subjectId)
        {
            return _context.Tests
                .Include(t => t.Subject)
                .Where(t => t.SubjectId == subjectId)
                .FirstOrDefault();
        }
    }
}

