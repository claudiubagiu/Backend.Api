using AutoMapper;
using Images.Api.Models.Domain;
using Images.Api.Models.DTOs;

namespace Images.Api.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UploadImageRequestDto, Image>()
            .ForMember(dest => dest.FileExtension, opt => opt.MapFrom(src => Path.GetExtension(src.File.FileName)))
            .ForMember(dest => dest.FileSizeInBytes, opt => opt.MapFrom(src => src.File.Length))
            .ForMember(dest => dest.FilePath, opt => opt.MapFrom(src => $"images/{src.FileName}{Path.GetExtension(src.File.FileName)}"));
            CreateMap<Image, ImageDto>().ReverseMap();
        }
    }
}
