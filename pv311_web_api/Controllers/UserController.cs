using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pv311_web_api.BLL.DTOs.User;
using pv311_web_api.BLL.Services.User;

namespace pv311_web_api.Controllers
{
    [ApiController]
    [Route("api/user")]
    [Authorize(Roles = "admin,manager", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class UserController : AppController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string? id)
        {
            var idValid = ValidateId(id, out string message);

            if (!idValid)
            {
                return BadRequest(message);
            }

            var response = await _userService.DeleteAsync(id ?? "");
            return CreateActionResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateUserDto dto)
        {
            var response = await _userService.CreateAsync(dto);
            return CreateActionResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(UpdateUserDto dto)
        {
            var response = await _userService.UpdateAsync(dto);
            return CreateActionResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(string? id, string? userName, string? email)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var response = await _userService.GetByIdAsync(id);
                return CreateActionResult(response);
            }
            else if (!string.IsNullOrEmpty(userName))
            {
                var response = await _userService.GetByUserNameAsync(userName);
                return CreateActionResult(response);
            }
            else if (!string.IsNullOrEmpty(email))
            {
                var response = await _userService.GetByEmailAsync(email);
                return CreateActionResult(response);
            }
            else
            {
                var response = await _userService.GetAllAsync();
                return CreateActionResult(response);
            }
        }
    }
}