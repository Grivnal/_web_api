using Microsoft.EntityFrameworkCore;
using pv311_web_api.DAL.Entities;

namespace pv311_web_api.DAL.Repositories.Manufactures
{
    public class ManufactureRepository
        : GenericRepository<Manufacture, string>,
        IManufactureRepository
    {
        private readonly AppDbContext _context;

        public ManufactureRepository(AppDbContext context) 
            : base(context)
        {
            _context = context;
        }

        public async Task<Manufacture?> GetByNameAsync(string name)
        {
            var entity = await _context.Manufactures
                .FirstOrDefaultAsync(e => e.Name.ToLower() == name.ToLower());
            return entity;
        }
    }
}