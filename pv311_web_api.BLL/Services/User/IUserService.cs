using pv311_web_api.BLL.DTOs.User;

namespace pv311_web_api.BLL.Services.User
{
    public interface IUserService
    {
        Task<ServiceResponse> CreateAsync(CreateUserDto dto);
        Task<ServiceResponse> UpdateAsync(UpdateUserDto dto);
        Task<ServiceResponse> DeleteAsync(string id);
        Task<ServiceResponse> GetAllAsync();
        Task<ServiceResponse> GetByIdAsync(string id);
        Task<ServiceResponse> GetByUserNameAsync(string userName);
        Task<ServiceResponse> GetByEmailAsync(string email);
    }
}