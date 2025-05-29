using AutoMapper;
using Posts.Api.Models;
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
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments.Where(c => c.CommentId == null)))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Votes, opt => opt.MapFrom(src => src.Votes))
                .ForMember(dest => dest.NrVotes, opt => opt.MapFrom(src => new NrVotes
                {
                    Likes = src.Votes.Count(v => v.Type == Posts.Api.Models.Type.Like),
                    Dislikes = src.Votes.Count(v => v.Type == Posts.Api.Models.Type.Dislike)
                }))
                .ReverseMap();
            CreateMap<AddPostRequestDto, Post>().ReverseMap();
            CreateMap<Post, UpdatePostRequestDto>().ReverseMap();

            // Tags
            CreateMap<Tag, TagDto>().ReverseMap();
            CreateMap<Tag, AddTagRequestDto>().ReverseMap();

            // Comments
            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.FeaturedImageUrl,
                    opt => opt.MapFrom(src => src.Image != null ? src.Image.FilePath : string.Empty))
                .ForMember(dest => dest.NrVotes,
                    opt => opt.MapFrom(src => new NrVotes
                    {
                        Likes = src.Votes.Count(v => v.Type == Posts.Api.Models.Type.Like),
                        Dislikes = src.Votes.Count(v => v.Type == Posts.Api.Models.Type.Dislike)
                    }))
                .ReverseMap();


            // User
            CreateMap<ApplicationUserDto, ApplicationUser>().ReverseMap();

            // Votes
            CreateMap<Vote, VoteDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ReverseMap();

            // Image
            CreateMap<Image, ImageDto>()
                .ReverseMap();
        }
    }
}
