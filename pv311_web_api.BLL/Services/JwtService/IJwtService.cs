using pv311_web_api.BLL.DTOs;
using pv311_web_api.DAL.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace pv311_web_api.BLL.Services.JwtService
{
    public interface IJwtService
    {
        string GenerateRefreshToken();
        Task<JwtSecurityToken> GenerateAccessTokenAsync(AppUser user);
        Task<JwtTokensDto?> GenerateTokensAsync(AppUser user);
        Task<ServiceResponse> RefreshTokensAsync(JwtTokensDto dto);
    }
}