using Microsoft.AspNetCore.Http;
using pv311_web_api.DAL.Entities;

namespace pv311_web_api.BLL.Services.Image
{
    public interface IImageService
    {
        Task<string?> SaveImageAsync(IFormFile image, string directoryPath);
        void DeleteImage(string filePath);
        void CreateDirectory(string path);
        Task<List<CarImage>> SaveCarImagesAsync(IEnumerable<IFormFile> images, string directoryPath);
    }
}