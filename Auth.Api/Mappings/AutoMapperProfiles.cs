using Auth.Models.Domain;
using Auth.Models.DTOs;
using AutoMapper;

namespace Auth.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RegisterRequestDto, ApplicationUser>().ReverseMap();
            CreateMap<ApplicationUser, LoginResponseDto>().ReverseMap();
        }
    }
}
