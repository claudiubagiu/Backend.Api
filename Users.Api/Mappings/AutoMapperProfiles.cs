using Users.Api.Models.Domain;
using AutoMapper;
using Users.Api.Models.DTOs;

namespace Users.Api.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ApplicationUser, ApplicationUserDto>()
                .ForMember(dest => dest.BanDto, opt => opt.MapFrom(src => src.Ban))
                .ReverseMap();
            CreateMap<Ban, BanDto>().ReverseMap();
            CreateMap<Ban, AddBanRequest>().ReverseMap();
        }
    }
}
