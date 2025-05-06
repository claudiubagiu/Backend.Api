using Comments.Api.Models.Domain;
using Comments.Api.Models.DTOs;
using AutoMapper;
using Microsoft.Extensions.Hosting;

namespace Comments.Api.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<Comment, AddCommentRequestDto>().ReverseMap();
            CreateMap<Comment, UpdateCommentRequestDto>().ReverseMap();
        }
    }
}
