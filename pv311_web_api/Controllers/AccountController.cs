using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using pv311_web_api.BLL.DTOs;
using pv311_web_api.BLL.DTOs.Account;
using pv311_web_api.BLL.Services.Account;
using pv311_web_api.BLL.Services.JwtService;

namespace pv311_web_api.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : AppController
    {
        private readonly IAccountService _accountService;
        private readonly IValidator<LoginDto> _loginValidator;
        private readonly IJwtService _jwtService;

        public AccountController(IAccountService accountService, IValidator<LoginDto> loginValidator, IJwtService jwtService)
        {
            _accountService = accountService;
            _loginValidator = loginValidator;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto dto)
        {
            var validResult = await _loginValidator.ValidateAsync(dto);

            if (!validResult.IsValid)
                return BadRequest(validResult);

            var user = await _accountService.LoginAsync(dto);

            return user == null ? BadRequest("Incorrect login or password") : Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto dto)
        {
            var result = await _accountService.RegisterAsync(dto);
            return result == null ? BadRequest("Register error") : Ok(result);
        }

        [HttpGet("emailConfirm")]
        public async Task<IActionResult> EmailConfirmAsync(string? id, string? t)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(t))
                return NotFound();

            var result = await _accountService.EmailConfirmAsync(id, t);

            if (!result)
                return NotFound();

            return Redirect("https://google.com");
        }

        [HttpGet("sendEmailConfirm")]
        public async Task<IActionResult> SendEmailConfirmAsync(string? userId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userId))
                return NotFound();

            var result = await _accountService.SendEmailConfirmAsync(userId);

            return result ? Ok() : BadRequest();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokensAsync(JwtTokensDto dto)
        {
            var response = await _jwtService.RefreshTokensAsync(dto);
            return CreateActionResult(response);
        }
    }
}