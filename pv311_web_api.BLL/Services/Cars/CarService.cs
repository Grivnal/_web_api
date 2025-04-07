using AutoMapper;
using Microsoft.EntityFrameworkCore;
using pv311_web_api.BLL.DTOs.Cars;
using pv311_web_api.BLL.Services.Image;
using pv311_web_api.DAL.Entities;
using pv311_web_api.DAL.Repositories.Cars;
using pv311_web_api.DAL.Repositories.Manufactures;

namespace pv311_web_api.BLL.Services.Cars
{
    public class CarService : ICarService
    {
        private readonly IMapper _mapper;
        private readonly ICarRepository _carRepository;
        private readonly IManufactureRepository _manufactureRepository;
        private readonly IImageService _imageService;

        public CarService(IMapper mapper, ICarRepository carRepository, IManufactureRepository manufactureRepository, IImageService imageService)
        {
            _mapper = mapper;
            _carRepository = carRepository;
            _manufactureRepository = manufactureRepository;
            _imageService = imageService;
        }

        public async Task<ServiceResponse> CreateAsync(CreateCarDto dto)
        {
            var entity = _mapper.Map<Car>(dto);

            if(!string.IsNullOrEmpty(dto.Manufacture))
            {
                entity.Manufacture = await _manufactureRepository
                    .GetByNameAsync(dto.Manufacture);
            }

            if(dto.Images.Count() > 0)
            {
                string path = Path.Combine(Settings.CarsPath, entity.Id);
                _imageService.CreateDirectory(path);
                var carImages = await _imageService.SaveCarImagesAsync(dto.Images, path);
                entity.Images = carImages;
            
            }

            var result = await _carRepository.CreateAsync(entity);

            if(!result)
            {
                return new ServiceResponse("Не вдалося збрегети автомобіль");
            }

            return new ServiceResponse($"Автомобіль '{entity.Brand} {entity.Model}' збережено", true);
        }

        public async Task<ServiceResponse> GetAllAsync(int page = 1, int pageSize = Settings.PageSize, string? manufacture = null)
        {
            pageSize = pageSize < 1 ? Settings.PageSize : pageSize;

            var cars = string.IsNullOrEmpty(manufacture)
                ? _carRepository.GetCars()
                : _carRepository.GetCars(c => c.Manufacture == null ? false : c.Manufacture.Name.ToLower() == manufacture.ToLower());

            int count = cars.Count();
            int pageCount = (int)Math.Ceiling((double)count / pageSize);

            page = page < 1 || page > pageCount ? 1 : page;

            cars = cars
                .Skip(pageSize * (page - 1))
                .Take(pageSize);

            var list = await cars.ToListAsync();

            var dtos = _mapper.Map<List<CarDto>>(list);

            var dtoList = new CarListDto
            {
                Cars = dtos,
                Page = page,
                PageCount = pageCount,
                TotalCount = count
            };

            return new ServiceResponse("Автомобілі отримано", true, dtoList);
        }

        public async Task<ServiceResponse> GetByPriceAsync(Range range, int page = 1, int pageSize = Settings.PageSize)
        {
            var cars = await _carRepository
                .GetCars(c => c.Price >= range.Start.Value && c.Price <= range.End.Value)
                .Include(c => c.Images)
                .Include(c => c.Manufacture)
                .ToListAsync();

            var dtos = _mapper.Map<List<CarDto>>(cars);

            return new ServiceResponse("Автомобілі отримано", true, dtos);
        }
    }
}