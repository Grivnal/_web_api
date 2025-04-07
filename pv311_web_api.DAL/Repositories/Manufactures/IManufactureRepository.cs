using pv311_web_api.DAL.Entities;

namespace pv311_web_api.DAL.Repositories.Manufactures
{
    public interface IManufactureRepository
        : IGenericRepository<Manufacture, string>
    {
        Task<Manufacture?> GetByNameAsync(string name);
    }
}