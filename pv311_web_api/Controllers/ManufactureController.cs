using Microsoft.AspNetCore.Mvc;
using pv311_web_api.BLL.DTOs.Manufactures;
using pv311_web_api.BLL.Services.Manufactures;

namespace pv311_web_api.Controllers
{
    [ApiController]
    [Route("api/manufacture")]
    public class ManufactureController : AppController
    {
        private readonly IManufactureService _manufactureService;
        private readonly ILogger<ManufactureController> _logger;

        public ManufactureController(IManufactureService manufactureService, ILogger<ManufactureController> logger)
        {
            _manufactureService = manufactureService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateManufactureDto dto)
        {
            var response = await _manufactureService.CreateAsync(dto);
            return CreateActionResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(UpdateManufactureDto dto)
        {
            var response = await _manufactureService.UpdateAsync(dto);
            return CreateActionResult(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string? id)
        {
            if(!ValidateId(id, out string error))
            {
                return BadRequest(error);
            }

            var response = await _manufactureService.DeleteAsync(id);
            return CreateActionResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(string? id)
        {
            var response = string.IsNullOrEmpty(id)
                ? await _manufactureService.GetAllAsync()
                : await _manufactureService.GetByIdAsync(id);
            return CreateActionResult(response);
        }
    }
}