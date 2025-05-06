using Images.Api.Models.Domain;
using Images.Api.Models.DTOs;
using Images.Api.Repositories.Interface;
using Images.Api.Services.Interface;
using AutoMapper;
using Azure.Core;
using System.Threading.Tasks.Dataflow;

namespace Images.Api.Services.Implementation
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository imageRepository;
        private readonly IMapper mapper;

        public ImageService(IImageRepository imageRepository, IMapper mapper)
        {
            this.imageRepository = imageRepository;
            this.mapper = mapper;
        }

        public async Task<Image> Upload(UploadImageRequestDto request)
        {
            ValidateFileUpload(request);
            if (request.File.Length > 0)
            {
                var imageDomainModel = mapper.Map<Image>(request);
                await imageRepository.Upload(imageDomainModel);
                return imageDomainModel;
            }
            return null;
        }

        private void ValidateFileUpload(UploadImageRequestDto request)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            var maxFileSizeInBytes = 10485760; // 10MB
            if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
            {
                throw new InvalidDataException("File extension is not allowed.");
            }
            if (request.File.Length > maxFileSizeInBytes)
            {
                throw new InvalidDataException("File size is too large.");
            }
        }

        public async Task<List<ImageDto>?> GetAllAsync()
        {
            var images = await imageRepository.GetAllAsync();
            return mapper.Map<List<ImageDto>>(images);
        }

        public async Task<ImageDto?> GetByPostIdAsync(Guid postId)
        {
            var image = await imageRepository.GetByPostIdAsync(postId);
            return mapper.Map<ImageDto>(image);
        }
        public async Task<ImageDto?> GetByCommentIdAsync(Guid commentId)
        {
            var image = await imageRepository.GetByCommentIdAsync(commentId);
            return mapper.Map<ImageDto>(image);
        }
        public async Task<Boolean> DeleteByPostIdAsync(Guid postId)
        {
            var image = await imageRepository.GetByPostIdAsync(postId);

            if (image == null)
                return false;

            await imageRepository.DeleteByPostIdAsync(postId);
            return true;
        }

        public async Task<Boolean> DeleteByCommentIdAsync(Guid commentId)
        {
            var image = await imageRepository.GetByCommentIdAsync(commentId);

            if (image == null)
                return false;

            await imageRepository.DeleteByCommentIdAsync(commentId);
            return true;
        }
    }
}
