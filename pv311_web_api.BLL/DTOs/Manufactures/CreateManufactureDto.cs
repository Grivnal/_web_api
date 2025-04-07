using Microsoft.AspNetCore.Http;
using pv311_web_api.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace pv311_web_api.BLL.DTOs.Manufactures
{
    public class CreateManufactureDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Founder { get; set; }
        public string? Director { get; set; }
        public IFormFile? Image { get; set; }
    }
}