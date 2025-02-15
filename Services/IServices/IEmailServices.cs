using Models.Email;

namespace Services.IServices
{
    public interface IEmailServices
    {
        Task<EmailModel> SendEmailAsync(SendEmailModel emailModel);
        Task<ConfirmEmailResponseModel> ConfirmEmailAsync(ConfirmEmailRequestModel emailRequestModel);
    }
}
