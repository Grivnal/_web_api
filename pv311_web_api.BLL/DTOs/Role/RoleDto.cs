using FluentValidation;

namespace pv311_web_api.BLL.DTOs.Role
{
    public class RoleDto
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
    }

    public class RoleValidator : AbstractValidator<RoleDto>
    {
        public RoleValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(256).WithMessage("Maximum length 256 symbols");
        }
    }
}