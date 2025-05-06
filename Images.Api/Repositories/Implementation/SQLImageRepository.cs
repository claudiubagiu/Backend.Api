using Images.Api.Data;
using Images.Api.Models.Domain;
using Images.Api.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Images.Api.Repositories.Implementation
{
    public class SQLImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly BackendDbContext dbContext;

        public SQLImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, BackendDbContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }

        public async Task<Image> Upload(Image image)
        {
            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images",
                $"{image.FileName}{image.FileExtension}");

            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            // https ://localhost:1234/images/image.jpg
            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";

            image.FilePath = urlFilePath;

            // Add image to the Images table
            await dbContext.Image.AddAsync(image);
            await dbContext.SaveChangesAsync();

            return image;
        }

        public async Task<List<Image>> GetAllAsync()
        {
            return await dbContext.Image.ToListAsync();
        }

        public async Task<Image> GetByPostIdAsync(Guid postId)
        {
            return await dbContext.Image.FirstOrDefaultAsync(x => x.PostId == postId);
        }

        public async Task<Image> GetByCommentIdAsync(Guid postId)
        {
            return await dbContext.Image.FirstOrDefaultAsync(x => x.CommentId == postId);
        }

        public async Task<Image?> DeleteByPostIdAsync(Guid postId)
        {
            var image = await dbContext.Image.FirstOrDefaultAsync(x => x.PostId == postId);

            if (image == null)
                return null;

            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images",
                $"{image.FileName}{image.FileExtension}");

            if (File.Exists(localFilePath))
            {
                File.Delete(localFilePath);
            }

            dbContext.Image.Remove(image);
            await dbContext.SaveChangesAsync();

            return image;
        }

        public async Task<Image?> DeleteByCommentIdAsync(Guid commentId)
        {
            var image = await dbContext.Image.FirstOrDefaultAsync(x => x.CommentId == commentId);

            if (image == null)
                return null;

            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images",
                $"{image.FileName}{image.FileExtension}");

            if (File.Exists(localFilePath))
            {
                File.Delete(localFilePath);
            }

            dbContext.Image.Remove(image);
            await dbContext.SaveChangesAsync();

            return image;
        }
    }
}
