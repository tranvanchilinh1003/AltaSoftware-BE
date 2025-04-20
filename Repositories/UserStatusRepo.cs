using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class UserStatusRepo
    {

        private readonly isc_dbContext _context;
        public UserStatusRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<UserStatus> GetUserStatuses()
        {
            return _context.UserStatuses.Where(us => !us.IsDeleted).ToList();
        }

        public UserStatus? GetUserStatusById(long id)
        {
            return _context.UserStatuses.FirstOrDefault(s => s.Id == id);
        }

        public UserStatus CreateUserStatus(UserStatus userStatus)
        {
            _context.UserStatuses.Add(userStatus);
            _context.SaveChanges();
            return userStatus;
        }

        public UserStatus UpdateUserStatus(UserStatus userStatus)
        {
            _context.UserStatuses.Update(userStatus);
            _context.SaveChanges();
            return userStatus;
        }
    }
}
