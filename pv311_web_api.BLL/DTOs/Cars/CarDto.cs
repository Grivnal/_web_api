using Microsoft.AspNetCore.Http;

namespace pv311_web_api.BLL.DTOs.Cars
{
    public class CarDto
    {
        public required string Id { get; set; }
        public required string Model { get; set; }
        public required string Brand { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public string? Gearbox { get; set; }
        public string? Color { get; set; }
        public string? Manufacture { get; set; }
        public List<string> Images { get; set; } = [];
    }
}