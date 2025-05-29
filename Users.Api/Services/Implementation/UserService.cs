using AutoMapper;
using Users.Api.Models.Domain;
using Users.Api.Models.DTOs;
using Users.Api.Repositories.Interface;
using Users.Api.Services.Interface;

namespace Users.Api.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<List<ApplicationUserDto>> GetAllAsync()
        {
            var users = await userRepository.GetAllAsync();
            if (users == null)
                return new List<ApplicationUserDto>();
            return mapper.Map<List<ApplicationUserDto>>(users);
        }

        public async Task<ApplicationUserDto?> GetByIdAsync(string id)
        {
            var user = await userRepository.GetByIdAsync(id);
            if (user == null)
                return null;
            return mapper.Map<ApplicationUserDto>(user);
        }

        public async Task<float> ModifyScore(ModifyScore modifyScore)
        {
            var user = await userRepository.GetByIdAsync(modifyScore.UserId);
            if (user == null)
                return 0;
            return await userRepository.ModifyScore(modifyScore.UserId, modifyScore.Score);
        }
    }
}
