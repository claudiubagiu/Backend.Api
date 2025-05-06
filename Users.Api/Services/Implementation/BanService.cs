using AutoMapper;
using System.Text.Json;
using System.Text;
using Users.Api.Models.Domain;
using Users.Api.Models.DTOs;
using Users.Api.Repositories.Interface;
using Users.Api.Services.Interface;

namespace Users.Api.Services.Implementation
{
    public class BanService : IBanService
    {
        private readonly IBanRepository banRepository;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IHttpClientFactory httpClientFactory;

        public BanService(IBanRepository banRepository, IUserRepository userRepository, IMapper mapper, IHttpClientFactory httpClientFactory)
        {
            this.banRepository = banRepository;
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<bool> BanUserAsync(AddBanRequest addBanRequest)
        {
            var ban = mapper.Map<Ban>(addBanRequest);
            var user = await userRepository.GetByIdAsync(ban.UserId);
            Notification notification = new Notification
            {
                Subject = "ConnexUs",
                Body = addBanRequest.Description,
                PhoneNumber = user.PhoneNumber,
                ToEmail = user.Email,
                Message = addBanRequest.Description
            };

            var client = httpClientFactory.CreateClient();
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(notification),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync($"http://notification.api:5007/api/Notification/send-notification", jsonContent);
            response.EnsureSuccessStatusCode();
            return await banRepository.BanUserAsync(ban);
        }

        public async Task<bool> IsUserBannedAsync(string userEmail)
        {
            return await banRepository.IsUserBannedAsync(userEmail);
        }

        public async Task<bool> UnbanUserAsync(string userId)
        {
            return await banRepository.UnbanUserAsync(userId);
        }
    }
}
