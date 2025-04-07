using AutoMapper;
using pv311_web_api.BLL.DTOs.Cars;
using pv311_web_api.DAL.Entities;

namespace pv311_web_api.BLL.MapperProfiles
{
    public class CarMapperProfile : Profile
    {
        public CarMapperProfile()
        {
            // CreateCarDto -> Car
            CreateMap<CreateCarDto, Car>()
                .ForMember(dest => dest.Manufacture, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore());

            // Car -> CarDto
            CreateMap<Car, CarDto>()
                .ForMember(dest => dest.Manufacture, opt => opt.MapFrom(src => src.Manufacture == null ? string.Empty : src.Manufacture.Name))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(i => Path.Combine(i.Path, i.Name))));
        }
    }
}