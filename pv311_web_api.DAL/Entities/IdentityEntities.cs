using Microsoft.AspNetCore.Identity;

namespace pv311_web_api.DAL.Entities
{
    public class AppUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Image { get; set; }

        public virtual ICollection<AppUserClaim> Claims { get; set; } = [];
        public virtual ICollection<AppUserLogin> Logins { get; set; } = [];
        public virtual ICollection<AppUserToken> Tokens { get; set; } = [];
        public virtual ICollection<AppUserRole> UserRoles { get; set; } = [];
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    }

    public class AppRole : IdentityRole
    {
        public virtual ICollection<AppUserRole> UserRoles { get; set; } = [];
        public virtual ICollection<AppRoleClaim> RoleClaims { get; set; } = [];
    }

    public class AppUserRole : IdentityUserRole<string>
    {
        public virtual AppUser? User { get; set; }
        public virtual AppRole? Role { get; set; }
    }

    public class AppUserClaim : IdentityUserClaim<string>
    {
        public virtual AppUser? User { get; set; }
    }

    public class AppUserLogin : IdentityUserLogin<string>
    {
        public virtual AppUser? User { get; set; }
    }

    public class AppRoleClaim : IdentityRoleClaim<string>
    {
        public virtual AppRole? Role { get; set; }
    }

    public class AppUserToken : IdentityUserToken<string>
    {
        public virtual AppUser? User { get; set; }
    }
}