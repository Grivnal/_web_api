using Microsoft.EntityFrameworkCore;
using pv311_web_api.DAL.Entities;
using System.Linq.Expressions;

namespace pv311_web_api.DAL.Repositories.Cars
{
    public class CarRepository
        : GenericRepository<Car, string>,
        ICarRepository
    {
        public CarRepository(AppDbContext context)
        : base(context) { }

        public IQueryable<Car> GetCars(Expression<Func<Car, bool>>? pred = null)
        {
            var entites = GetAll()
                .Include(c => c.Manufacture)
                .Include(c => c.Images);

            if (pred != null)
            {
                return entites.Where(pred);
            }

            return entites;
        }
    }
}