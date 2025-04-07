using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using pv311_web_api.BLL.DTOs;
using pv311_web_api.DAL.Entities;
using pv311_web_api.DAL.Repositories.JwtRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace pv311_web_api.BLL.Services.JwtService
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtRepository _jwtRepository;

        public JwtService(IConfiguration configuration, UserManager<AppUser> userManager, IJwtRepository jwtRepository)
        {
            _configuration = configuration;
            _userManager = userManager;
            _jwtRepository = jwtRepository;
        }

        public async Task<JwtSecurityToken> GenerateAccessTokenAsync(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", user.Id),
                new Claim("email", user.Email ?? ""),
                new Claim("userName", user.UserName ?? ""),
                new Claim("image", user.Image ?? "")
            };

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Any())
            {
                var roleClaims = roles.Select(r => new Claim("role", r));
                claims.AddRange(roleClaims);
            }

            string? audience = _configuration["JwtSettings:Audience"];
            string? issuer = _configuration["JwtSettings:Issuer"];
            string? secretKey = _configuration["JwtSettings:SecretKey"];
            int expMinutes = int.Parse(_configuration["JwtSettings:ExpMinutes"] ?? "");

            if (audience == null || issuer == null || secretKey == null)
            {
                throw new ArgumentNullException("Jwt settings not found");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims.ToArray(),
                expires: DateTime.UtcNow.AddMinutes(expMinutes),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        public string GenerateRefreshToken()
        {
            var bytes = new byte[64];

            using var rnd = RandomNumberGenerator.Create();
            rnd.GetNonZeroBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        public async Task<JwtTokensDto?> GenerateTokensAsync(AppUser user)
        {
            var accessToken = await GenerateAccessTokenAsync(user);
            var refreshToken = GenerateRefreshToken();

            await SaveRefreshTokenAsync(user.Id, refreshToken, accessToken.Id);

            var dto = new JwtTokensDto
            {
                RefreshToken = refreshToken,
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken)
            };

            return dto;
        }

        private async Task SaveRefreshTokenAsync(string userId, string refreshToken, string accessTokenId)
        {
            var entity = new RefreshToken
            {
                AccessId = accessTokenId,
                Token = refreshToken,
                UserId = userId
            };

            await _jwtRepository.CreateAsync(entity);
        }

        private async Task<IEnumerable<Claim>> GetClaimsAsync(string accessToken)
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["JwtSettings:Issuer"],
                ValidAudience = _configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"] ?? "")),
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var result = await tokenHandler.ValidateTokenAsync(accessToken, validationParameters);

            if(!result.IsValid)
            {
                throw new SecurityTokenException("Token is not valid");
            }

            var token = result.SecurityToken as JwtSecurityToken;

            if(token == null || !token.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
            {
                throw new SecurityTokenException("Invalid token algorithm");
            }

            return token.Claims;
        }

        private bool ValidRefreshToken(RefreshToken token)
        {
            if(token.IsUsed)
            {
                throw new SecurityTokenValidationException("Token is used");
            }
            if(token.ExpiredTime < DateTime.UtcNow)
            {
                throw new SecurityTokenExpiredException("Token expired");
            }

            return true;
        }

        public async Task<ServiceResponse> RefreshTokensAsync(JwtTokensDto dto)
        {
            var refreshToken = await _jwtRepository.GetByTokenAsync(dto.RefreshToken);

            if (refreshToken == null)
            {
                return new ServiceResponse("Incorrect refresh token");
            }

            try
            {
                ValidRefreshToken(refreshToken);
            }
            catch(SecurityTokenExpiredException ex)
            {
                return new ServiceResponse(ex.Message);
            }
            catch (SecurityTokenValidationException ex)
            {
                return new ServiceResponse(ex.Message);
            }

            try
            {
                var claims = await GetClaimsAsync(dto.AccessToken);
                string accessId = claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

                if (refreshToken.AccessId != accessId)
                {
                    return new ServiceResponse("Incorrect access token");
                }
            }
            catch (SecurityTokenException ex)
            {
                return new ServiceResponse(ex.Message);
            }

            var user = await _userManager.FindByIdAsync(refreshToken.UserId);

            if(user == null)
            {
                return new ServiceResponse("Refresh tokens error. User not found");
            }

            refreshToken.IsUsed = true;
            await _jwtRepository.UpdateAsync(refreshToken);

            var tokens = await GenerateTokensAsync(user);

            return new ServiceResponse("Токени оновлено", true, tokens);
        }
    }
}