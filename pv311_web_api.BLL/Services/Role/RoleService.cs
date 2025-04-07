using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pv311_web_api.BLL.DTOs.Role;
using pv311_web_api.DAL.Entities;

namespace pv311_web_api.BLL.Services.Role
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleService(RoleManager<AppRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> CreateAsync(RoleDto dto)
        {
            if(await _roleManager.RoleExistsAsync(dto.Name))
            {
                return new ServiceResponse($"Роль '{dto.Name}' вже існує");
            }

            var entity = _mapper.Map<AppRole>(dto);

            var result = await _roleManager.CreateAsync(entity);
            
            if(result.Succeeded)
            {
                return new ServiceResponse($"Роль '{dto.Name}' успішно створено", true);
            }

            return new ServiceResponse(result.Errors.First().Description);
        }

        public async Task<ServiceResponse> DeleteAsync(string id)
        {
            var entity = await _roleManager.FindByIdAsync(id);

            if (entity == null)
            {
                return new ServiceResponse($"Роль не знайдено");
            }

            var result = await _roleManager.DeleteAsync(entity);

            if (result.Succeeded)
            {
                return new ServiceResponse($"Роль '{entity.Name}' успішно видалено", true);
            }

            return new ServiceResponse(result.Errors.First().Description);
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            var entities = await _roleManager.Roles.ToListAsync();

            var dtos = _mapper.Map<List<RoleDto>>(entities);

            return new ServiceResponse("Ролі отримано", true, dtos);
        }

        public async Task<ServiceResponse> GetByIdAsync(string id)
        {
            var entity = await _roleManager.FindByIdAsync(id);

            if (entity == null)
                return new ServiceResponse("Роль не знайдено");

            var dto = _mapper.Map<RoleDto>(entity);

            return new ServiceResponse("Роль отримано", true, dto);
        }

        public async Task<ServiceResponse> UpdateAsync(RoleDto dto)
        {
            if (await _roleManager.RoleExistsAsync(dto.Name))
            {
                return new ServiceResponse($"Роль '{dto.Name}' вже існує");
            }

            var role = await _roleManager.FindByIdAsync(dto.Id);

            if(role == null)
            {
                return new ServiceResponse($"Роль {dto.Id} не знайдено");
            }

            var entity = _mapper.Map(dto, role);

            var result = await _roleManager.UpdateAsync(entity);

            if (result.Succeeded)
            {
                return new ServiceResponse($"Роль '{dto.Name}' успішно оновлено", true);
            }

            return new ServiceResponse(result.Errors.First().Description);
        }
    }
}