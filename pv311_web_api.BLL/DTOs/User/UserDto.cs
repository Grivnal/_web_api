using pv311_web_api.BLL.DTOs.Role;

namespace pv311_web_api.BLL.DTOs.User
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Image { get; set; }
        public IEnumerable<RoleDto> Roles { get; set; } = [];
    }
}