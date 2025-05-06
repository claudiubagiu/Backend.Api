using AutoMapper;
using Posts.Api.Models.Domain;
using Posts.Api.Models.DTOs;

namespace Posts.Api.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Posts
            CreateMap<Post, PostDto>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
                .ReverseMap();
            CreateMap<AddPostRequestDto, Post>().ReverseMap();
            CreateMap<Post, UpdatePostRequestDto>().ReverseMap();

            //  Tags
            CreateMap<Tag, TagDto>().ReverseMap();
            CreateMap<Tag, AddTagRequestDto>().ReverseMap();

        }
    }
}
