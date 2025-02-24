using Microsoft.AspNetCore.Http;

namespace Services.Abstractios
{
    public interface IFileServices
    {
        Task<string> UploadFile(string location, IFormFile file);
    }
}
