using Models.Email;

namespace Services.IServices
{
    public interface IEmailServices
    {
        Task<EmailModel> SendEmailAsync(SendEmailModel sendEmailModel);
        Task<EmailResponse> ConfirmEmailAsync(EmailRequest emailRequest);
    }
}
