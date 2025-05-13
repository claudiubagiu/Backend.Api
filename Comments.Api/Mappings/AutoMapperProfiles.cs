using Comments.Api.Models.Domain;
using Comments.Api.Models.DTOs;
using AutoMapper;
using Microsoft.Extensions.Hosting;
using Comments.Api.Models;

namespace Comments.Api.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Comments
            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.Votes, opt => opt.MapFrom(src => src.Votes))
                .ForMember(dest => dest.NrVotes, opt => opt.MapFrom(src => new NrVotes
                {
                    Likes = src.Votes.Count(v => v.Type == Comments.Api.Models.Type.Like),
                    Dislikes = src.Votes.Count(v => v.Type == Comments.Api.Models.Type.Dislike)
                }))
                .ReverseMap();
            CreateMap<Comment, AddCommentRequestDto>().ReverseMap();
            CreateMap<Comment, UpdateCommentRequestDto>().ReverseMap();

            // Votes
            CreateMap<Vote, VoteDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ReverseMap();

            // User
            CreateMap<ApplicationUserDto, ApplicationUser>().ReverseMap();

            // Image
            CreateMap<Image, ImageDto>()
                .ReverseMap();
        }
    }
}
