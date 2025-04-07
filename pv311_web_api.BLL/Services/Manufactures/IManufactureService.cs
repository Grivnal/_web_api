using pv311_web_api.BLL.DTOs.Manufactures;

namespace pv311_web_api.BLL.Services.Manufactures
{
    public interface IManufactureService
    {
        Task<ServiceResponse> CreateAsync(CreateManufactureDto dto);
        Task<ServiceResponse> DeleteAsync(string id);
        Task<ServiceResponse> UpdateAsync(UpdateManufactureDto dto);
        Task<ServiceResponse> GetByIdAsync(string id);
        Task<ServiceResponse> GetAllAsync();
    }
}