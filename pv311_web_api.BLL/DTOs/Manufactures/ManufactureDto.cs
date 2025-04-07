using Microsoft.AspNetCore.Http;

namespace pv311_web_api.BLL.DTOs.Manufactures
{
    public class ManufactureDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Founder { get; set; }
        public string? Director { get; set; }
        public string? Image { get; set; }
    }
}