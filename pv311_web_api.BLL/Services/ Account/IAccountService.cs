using pv311_web_api.BLL.DTOs.Account;

namespace pv311_web_api.BLL.Services.Account
{
    public interface IAccountService
    {
        Task<ServiceResponse> LoginAsync(LoginDto dto);
        Task<ServiceResponse> RegisterAsync(RegisterDto dto);
        Task<bool> EmailConfirmAsync(string id, string token);
        Task<bool> SendEmailConfirmAsync(string userId);
    }
}