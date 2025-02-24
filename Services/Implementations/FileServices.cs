using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Services.Abstractios;

namespace Services.Implementations
{
    public class FileServices : IFileServices
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<FileServices> _logger;

        public FileServices(IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment webHostEnvironment,
            ILogger<FileServices> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public async Task<string> UploadFile(string location, IFormFile file)
        {
            if (string.IsNullOrEmpty(location))
            {
                return "InvalidLocation";
            }

            var path = Path.Combine(_webHostEnvironment.WebRootPath, location);
            var extension = Path.GetExtension(file.FileName);
            if (string.IsNullOrEmpty(extension))
            {
                return "Invalid File Extension";
            }

            var fileName = $"{Guid.NewGuid():N}{extension}";
            var fileFullPath = Path.Combine(path, fileName);

            if (file.Length > 0)
            {
                try
                {
                    if (!File.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    using (var filestream = new FileStream(fileFullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(filestream);
                        await filestream.FlushAsync();
                    }

                    var request = _httpContextAccessor.HttpContext?.Request;
                    var baseUrl = request != null ? $"{request.Scheme}://{request.Host}" : "";
                    var relativePath = $"{location}/{fileName}";
                    var fullUrl = $"{baseUrl}{relativePath}";
                    return fullUrl;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Faild To Upload Image");
                    return "Faild To Upload Image";
                }
            }
            else
            {
                _logger.LogWarning("NoImage");
                return "NoImage";
            }
        }
    }
}
