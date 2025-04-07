using AutoMapper;
using pv311_web_api.BLL.DTOs.Account;
using pv311_web_api.BLL.DTOs.User;
using pv311_web_api.DAL.Entities;

namespace pv311_web_api.BLL.MapperProfiles
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            // RegisterDto -> AppUser
            CreateMap<RegisterDto, AppUser>();

            // AppUser -> UserDto
            CreateMap<AppUser, UserDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles));

            // CreateUserDto -> AppUser
            CreateMap<CreateUserDto, AppUser>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            // UpdateUserDto -> AppUser
            CreateMap<UpdateUserDto, AppUser>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());
        }
    }
}