using Microsoft.AspNetCore.Http;

namespace Models.Email
{
    public class SendEmailModel
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public IReadOnlyList<IFormFile>? Attachments { get; set; }
    }
}
