namespace pv311_web_api.BLL.DTOs
{
    public class JwtTokensDto
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}