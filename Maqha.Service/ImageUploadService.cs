using Maqha.Core.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maqha.Service
{
    public class ImageUploadService : IImageUploadService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ImageUploadService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
        private readonly long _maxFileSize = 5 * 1024 * 1024; // 5MB

        public ImageUploadService(IWebHostEnvironment environment, ILogger<ImageUploadService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _environment = environment;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> DeleteImageAsync(string imageUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(imageUrl))
                    return false;
                // Convert relative path to physical path
                var physicalPath = Path.Combine(_environment.WebRootPath, imageUrl.TrimStart('/')).Replace('/', Path.DirectorySeparatorChar);
                if (!File.Exists(physicalPath))
                {
                    _logger.LogWarning("Image not found: {ImageUrl}", imageUrl);
                    return false;
                }
                if (File.Exists(physicalPath))
                {
                    File.Delete(physicalPath);
                    _logger.LogInformation("Image deleted successfully: {ImageUrl}", imageUrl);
                    return true;
                }
                else
                {
                    _logger.LogWarning("Image file does not exist: {ImageUrl}", imageUrl);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting image: {ImageUrl}", imageUrl);
                throw;
            }
        }

        public bool IsValidImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return false;

            // Check file size
            if (imageFile.Length > _maxFileSize)
                return false;

            // Check file extension
            var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
                return false;

            // Check MIME type
            var allowedMimeTypes = new[]
            {
                "image/jpeg",
                "image/jpg",
                "image/png",
                "image/gif",
                "image/bmp",
                "image/webp"
            };

            if (!allowedMimeTypes.Contains(imageFile.ContentType.ToLowerInvariant()))
                return false;

            return true;
        }

        public async Task<string> UploadImageAsync(IFormFile imageFile, string folder = "images")
        {
            try
            {
                if (!IsValidImage(imageFile))
                {
                    throw new ArgumentException("Invalid image file type or size.");
                }
                // the web root path from the environment
                var webRootPath = _environment.WebRootPath;
                if (string.IsNullOrEmpty(webRootPath))
                {
                    webRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
                }
                //Create Upload Directory if not exists
                var uploadPath = Path.Combine(webRootPath, "uploads", folder);
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                //Generate unique file name to avoid conflicts
                var FileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                var uniqueFileName = $"{Guid.NewGuid()}{FileExtension}";
                var filePath = Path.Combine(uploadPath, uniqueFileName);
                //save the file to the server
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                  await  imageFile.CopyToAsync(fileStream);
                }
                // Rtehurn the URL of the uploaded image
                var request = _httpContextAccessor.HttpContext?.Request;
                if (request != null)
                {
                    var baseUrl = $"{request.Scheme}://{request.Host}";
                    var fullImageUrl = $"{baseUrl}/uploads/{folder}/{uniqueFileName}";
                    _logger.LogInformation($"Image uploaded successfully: {fullImageUrl}");
                    return fullImageUrl;
                }
                else
                {
                    var relativePath = $"/uploads/{folder}/{uniqueFileName}";
                    _logger.LogInformation("Image uploaded successfully: {FilePath}", relativePath);
                    return relativePath;
                }
           
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading image: {FileName}", imageFile?.FileName);
                throw;
            }
        }
    }
}
