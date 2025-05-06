using AutoMapper;
using Votes.Api.Models;
using Votes.Api.Models.DTOs;

namespace Votes.Api.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Vote, VoteDto>().ReverseMap();
            CreateMap<Vote, VoteRequest>().ReverseMap();
        }
    }
}
