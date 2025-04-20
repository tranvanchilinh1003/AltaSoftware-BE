using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class RefreshTokenRepo
    {
        private readonly isc_dbContext _context;
        public RefreshTokenRepo(isc_dbContext context)
        {
            _context = context;
        }

        public RefreshToken Create(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Add(refreshToken);
            _context.SaveChanges();
            return refreshToken;
        }

        public RefreshToken Update(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Update(refreshToken);
            _context.SaveChanges();
            return refreshToken;
        }

        public RefreshToken GetById(int id)
        {
            return _context.RefreshTokens
                .FirstOrDefault(s => s.Id == id);
        }
    }
}
