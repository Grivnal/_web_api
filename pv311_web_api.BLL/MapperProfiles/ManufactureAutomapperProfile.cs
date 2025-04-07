using AutoMapper;
using pv311_web_api.BLL.DTOs.Manufactures;
using pv311_web_api.DAL.Entities;

namespace pv311_web_api.BLL.MapperProfiles
{
    public class ManufactureAutomapperProfile : Profile
    {
        public ManufactureAutomapperProfile()
        {
            // CreateManufactureDto -> Manufacture
            CreateMap<CreateManufactureDto, Manufacture>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.Cars, opt => opt.Ignore());

            // UpdateManufactureDto -> Manufacture
            CreateMap<UpdateManufactureDto, Manufacture>()
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.Cars, opt => opt.Ignore());

            // Manufacture -> ManufactureDto
            CreateMap<Manufacture, ManufactureDto>();
        }
    }
}