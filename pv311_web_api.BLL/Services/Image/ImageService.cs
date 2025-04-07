using Microsoft.AspNetCore.Http;
using pv311_web_api.DAL.Entities;

namespace pv311_web_api.BLL.Services.Image
{
    public class ImageService : IImageService
    {
        private readonly string ImagesPath;

        public ImageService()
        {
            ImagesPath = Path.Combine(Settings.FilesRootPath, "wwwroot", Settings.ImagesPath);
        }

        public void CreateDirectory(string path)
        {
            path = Path.Combine(ImagesPath, path);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public void DeleteImage(string filePath)
        {
            try
            {
                string path = Path.Combine(ImagesPath, filePath);

                if(File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<List<CarImage>> SaveCarImagesAsync(IEnumerable<IFormFile> images, string directoryPath)
        {
            var carImages = new List<CarImage>();

            foreach (var image in images)
            {
                var imageName = await SaveImageAsync(image, directoryPath);
                if (imageName != null)
                {
                    var carImage = new CarImage
                    {
                        Name = imageName,
                        Path = directoryPath
                    };
                    carImages.Add(carImage);
                }
                
            }

            return carImages;
        }

        public async Task<string?> SaveImageAsync(IFormFile image, string directoryPath)
        {
            try
            {
                var types = image.ContentType.Split("/");
                if (types[0] != "image")
                {
                    return null;
                }

                string workPath = Path.Combine(ImagesPath, directoryPath);
                string imageName = $"{Guid.NewGuid()}.{types[1]}";
                string imagePath = Path.Combine(workPath, imageName);

                if (!Directory.Exists(workPath))
                {
                    Directory.CreateDirectory(workPath);
                }

                using (var stream = File.Create(imagePath))
                {
                    await image.CopyToAsync(stream);
                }

                return imageName;
            }
            catch (Exception)
            {
                return null;
            }
            
            
        }
    }
}