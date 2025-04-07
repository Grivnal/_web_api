using Microsoft.EntityFrameworkCore;
using pv311_web_api.DAL.Entities;

namespace pv311_web_api.DAL.Repositories.JwtRepository
{
    public class JwtRepository
        : GenericRepository<RefreshToken, string>, IJwtRepository
    {
        private readonly AppDbContext _context;

        public JwtRepository(AppDbContext context)
            : base(context) 
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == token);
        }
    }
}