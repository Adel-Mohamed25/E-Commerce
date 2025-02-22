using Models.Email;

namespace Services.Abstractions
{
    public interface IEmailServices
    {
        Task<EmailModel> SendEmailAsync(SendEmailModel sendEmailModel);
        Task<EmailResponse> ConfirmEmailAsync(EmailRequest emailRequest);
    }
}
