using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using pv311_web_api.BLL.DTOs.Manufactures;
using pv311_web_api.BLL.Services.Image;
using pv311_web_api.DAL.Entities;
using pv311_web_api.DAL.Repositories.Manufactures;
using System.Text;

namespace pv311_web_api.BLL.Services.Manufactures
{
    public class ManufactureService : IManufactureService
    {
        private readonly IMapper _mapper;
        private readonly IManufactureRepository _manufactureRepository;
        private readonly IImageService _imageService;
        private readonly ILogger<ManufactureService> _logger;

        public ManufactureService(IMapper mapper, IImageService imageService, IManufactureRepository manufactureRepository, ILogger<ManufactureService> logger)
        {
            _mapper = mapper;
            _imageService = imageService;
            _manufactureRepository = manufactureRepository;
            _logger = logger;
        }

        public async Task<ServiceResponse> CreateAsync(CreateManufactureDto dto)
        {
            var entity = _mapper.Map<Manufacture>(dto);
            string? imageName = null;

            if (dto.Image != null)
            {
                imageName = await _imageService.SaveImageAsync(dto.Image, Settings.ManufacturesPath);
                if (imageName != null)
                {
                    imageName = Path.Combine(Settings.ManufacturesPath, imageName);
                }
            }

            entity.Image = imageName;

            var result = await _manufactureRepository.CreateAsync(entity);
            
            if(result)
            {
                return new ServiceResponse($"Виробника {dto.Name} додано", true);
            }

            return new ServiceResponse("Помилка збереження виробника");
        }

        public async Task<ServiceResponse> DeleteAsync(string id)
        {
            var entity = await _manufactureRepository.GetByIdAsync(id);
            if(entity == null)
            {
                return new ServiceResponse($"Виробника з id {id} не знайдено");
            }

            if(!string.IsNullOrEmpty(entity.Image))
            {
                _imageService.DeleteImage(entity.Image);
            }

            var result = await _manufactureRepository.DeleteAsync(entity);

            if (result)
            {
                return new ServiceResponse($"Виробника {entity.Name} видалено", true);
            }

            return new ServiceResponse("Помилка видалення виробника");
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            var entity = await _manufactureRepository
                .GetAll()
                .ToListAsync();
            var dtos = _mapper.Map<List<ManufactureDto>>(entity);

            string manufactureNames = string.Join(", ", dtos.Select(x => x.Name));
            string date = DateTime.Now.ToString("dd.MM.yyyy HH:mm: ss: fff");

            string logMessage = $"Manufactures recived - {date}: {manufactureNames}";
            _logger.LogInformation(new EventId(10), logMessage);

            return new ServiceResponse("Виробники отримано", true, dtos);
        }

        public async Task<ServiceResponse> GetByIdAsync(string id)
        {
            var entity = await _manufactureRepository.GetByIdAsync(id);

            if (entity == null)
            {
                return new ServiceResponse($"Виробника з id {id} не знайдено");
            }

            var dto = _mapper.Map<ManufactureDto>(entity);
            return new ServiceResponse("Виробника отримано", true, dto);
        }

        public async Task<ServiceResponse> UpdateAsync(UpdateManufactureDto dto)
        {
            var entity = await _manufactureRepository
                .GetAll()
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == dto.Id);

            if (entity == null)
            {
                return new ServiceResponse($"Виробника з id {dto.Id} не знайдено");
            }
            entity = _mapper.Map(dto, entity);

            if(dto.Image != null)
            {
                var imageName = await _imageService.SaveImageAsync(dto.Image, Settings.ManufacturesPath);
                if (!string.IsNullOrEmpty(imageName))
                {
                    imageName = Path.Combine(Settings.ManufacturesPath, imageName);
                }

                if (!string.IsNullOrEmpty(entity.Image) && !string.IsNullOrEmpty(imageName))
                {
                    _imageService.DeleteImage(entity.Image);
                }

                entity.Image = imageName;
            }

            var result = await _manufactureRepository.UpdateAsync(entity);

            if (result)
            {
                return new ServiceResponse($"Виробника {dto.Name} оновлено", true);
            }

            return new ServiceResponse("Помилка оновлення виробника");
        }
    }
}