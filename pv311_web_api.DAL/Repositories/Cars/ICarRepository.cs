using pv311_web_api.DAL.Entities;
using System.Linq.Expressions;

namespace pv311_web_api.DAL.Repositories.Cars
{
    public interface ICarRepository
        : IGenericRepository<Car, string>
    {
        IQueryable<Car> GetCars(Expression<Func<Car, bool>>? pred = null);
    }
}