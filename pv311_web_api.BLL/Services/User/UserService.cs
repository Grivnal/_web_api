using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pv311_web_api.BLL.DTOs.User;
using pv311_web_api.BLL.Services.Image;
using pv311_web_api.DAL.Entities;
using System.Linq.Expressions;

namespace pv311_web_api.BLL.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public UserService(UserManager<AppUser> userManager, IMapper mapper, IImageService imageService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<ServiceResponse> CreateAsync(CreateUserDto dto)
        {
            if(!await UniqueEmailAsync(dto.Email))
            {
                return new ServiceResponse($"Користувач з поштою '{dto.Email}' вже існує");
            }
            if (!await UniqueUserNameAsync(dto.UserName))
            {
                return new ServiceResponse($"Користувач з іменем '{dto.UserName}' вже існує");
            }

            var entity = _mapper.Map<AppUser>(dto);

            if(dto.Image != null)
            {
                var imageName = await _imageService.SaveImageAsync(dto.Image, Settings.UsersPath);
                if(!string.IsNullOrEmpty(imageName))
                {
                    entity.Image = Path.Combine(Settings.UsersPath, imageName);
                }
            }

            var result = await _userManager.CreateAsync(entity, dto.Password);

            if (!result.Succeeded)
            {
                return new ServiceResponse(result.Errors.First().Description);
            }

            result = await _userManager.AddToRolesAsync(entity, dto.Roles);

            if (!result.Succeeded)
            {
                return new ServiceResponse(result.Errors.First().Description);
            }

            return new ServiceResponse($"Користувача '{entity.UserName}' створено", true);
        }

        public async Task<ServiceResponse> DeleteAsync(string id)
        {
            var entity = await _userManager.FindByIdAsync(id);
            if (entity == null)
            {
                return new ServiceResponse("Користувача не знайдено");
            }

            var result = await _userManager.DeleteAsync(entity);

            if(!result.Succeeded)
            {
                return new ServiceResponse(result.Errors.First().Description);
            }

            if(!string.IsNullOrEmpty(entity.Image))
            {
                _imageService.DeleteImage(entity.Image);
            }

            return new ServiceResponse($"Користувач '{entity.UserName}' успішно видалений", true);
        }

        public async Task<ServiceResponse> GetByEmailAsync(string email)
        {
            return await GetUserAsync(u => u.NormalizedEmail == email.ToUpper());
        }

        public async Task<ServiceResponse> GetByIdAsync(string id)
        {
            return await GetUserAsync(u => u.Id == id);
        }

        public async Task<ServiceResponse> GetByUserNameAsync(string userName)
        {
            return await GetUserAsync(u => u.NormalizedUserName == userName.ToUpper());
        }

        public async Task<ServiceResponse> UpdateAsync(UpdateUserDto dto)
        {
            var entity = await _userManager.FindByIdAsync(dto.Id);

            if(entity == null)
            {
                return new ServiceResponse("Не вдалося оновити користувача");
            }

            if(entity.NormalizedEmail != dto.Email.ToUpper())
            {
                var emailRes = await _userManager.SetEmailAsync(entity, dto.Email);
                if (!emailRes.Succeeded)
                {
                    return new ServiceResponse(emailRes.Errors.First().Description);
                }
            }

            if (entity.NormalizedUserName != dto.UserName.ToUpper())
            {
                var nameRes = await _userManager.SetUserNameAsync(entity, dto.UserName);
                if (!nameRes.Succeeded)
                {
                    return new ServiceResponse(nameRes.Errors.First().Description);
                }
            }

            entity = _mapper.Map(dto, entity);

            // image -->
            if(dto.Image != null)
            {
                if(!string.IsNullOrEmpty(entity.Image))
                {
                    _imageService.DeleteImage(entity.Image);
                }

                var imageName = await _imageService.SaveImageAsync(dto.Image, Settings.UsersPath);
                if(!string.IsNullOrEmpty(imageName))
                {
                    entity.Image = Path.Combine(Settings.UsersPath, imageName);
                }
            }
            // <-- image

            var result = await _userManager.UpdateAsync(entity);

            if (!result.Succeeded)
            {
                return new ServiceResponse(result.Errors.First().Description);
            }

            // roles -->
            var userRoles = await _userManager.GetRolesAsync(entity);

            var deleteRoles = userRoles.Where(r => !dto.Roles.Contains(r));

            if (deleteRoles.Any())
            {
                var deleteRes = await _userManager.RemoveFromRolesAsync(entity, deleteRoles);
                if (!deleteRes.Succeeded)
                {
                    return new ServiceResponse(deleteRes.Errors.First().Description);
                }
            }

            var newRoles = dto.Roles.Where(r => !userRoles.Contains(r));
            if (newRoles.Any())
            {
                var addRes = await _userManager.AddToRolesAsync(entity, newRoles);
                if (!addRes.Succeeded)
                {
                    return new ServiceResponse(addRes.Errors.First().Description);
                }
            }
            // <-- roles

            return new ServiceResponse($"Користувача '{entity.UserName}' змінено", true);
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            var entities = await _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .AsNoTracking()
                .ToListAsync();

            var dtos = _mapper.Map<List<UserDto>>(entities);
            return new ServiceResponse("Список користувачів отримано", true, dtos);
        }

        private async Task<ServiceResponse> GetUserAsync(Expression<Func<AppUser, bool>> predicate)
        {
            var entity = await _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(predicate);
            if (entity == null)
            {
                return new ServiceResponse("Користувача не знайдено");
            }

            var dto = _mapper.Map<UserDto>(entity);
            return new ServiceResponse("Користувача отримано", true, dto);
        }

        private async Task<bool> UniqueEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) == null;
        }

        private async Task<bool> UniqueUserNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName) == null;
        }
    }
}