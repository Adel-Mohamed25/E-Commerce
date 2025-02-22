using Infrastructure.Settings;
using Infrastructure.UnitOfWorks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Email;
using Services.Abstractions;
using System.Net;
using System.Net.Mail;

namespace Services.Implementations
{
    public class EmailServices : IEmailServices
    {
        private readonly EmailSettings _emailSettings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EmailServices> _logger;

        public EmailServices(IOptions<EmailSettings> emailSettings, IUnitOfWork unitOfWork, ILogger<EmailServices> logger)
        {
            _emailSettings = emailSettings.Value;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<EmailModel> SendEmailAsync(SendEmailModel sendEmailModel)
        {
            var emailResponse = new EmailModel
            {
                To = sendEmailModel.To,
                Subject = sendEmailModel.Subject,
                Body = sendEmailModel.Body,
                From = _emailSettings.From
            };

            try
            {
                using var smtpClient = new SmtpClient();
                smtpClient.Host = _emailSettings.Host;
                smtpClient.Port = _emailSettings.Port;
                smtpClient.Credentials = new NetworkCredential(_emailSettings.From, _emailSettings.Password);
                smtpClient.EnableSsl = true;

                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.From, _emailSettings.DisplayName),
                    Subject = sendEmailModel.Subject,
                    Body = sendEmailModel.Body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(sendEmailModel.To);

                if (sendEmailModel.Attachments is not null)
                {
                    foreach (var attachments in sendEmailModel.Attachments)
                    {
                        using var stream = attachments.OpenReadStream();
                        using var memoryStream = new MemoryStream();
                        await stream.CopyToAsync(memoryStream);
                        mailMessage.Attachments.Add(new Attachment(new MemoryStream(memoryStream.ToArray()), attachments.FileName));
                    }
                }

                await smtpClient.SendMailAsync(mailMessage);
                emailResponse.IsSuccess = true;
            }
            catch (SmtpException ex)
            {
                emailResponse.IsSuccess = false;
                _logger.LogError(ex, $"An error occurred while sending the email to{sendEmailModel.To}");
            }

            return emailResponse;
        }

        public async Task<EmailResponse> ConfirmEmailAsync(EmailRequest emailRequest)
        {
            var emailResponse = new EmailResponse
            {
                UserId = emailRequest.UserId,
            };

            var user = await _unitOfWork.Users.UserManager.FindByIdAsync(emailRequest.UserId);
            if (user == null)
            {
                emailResponse.IsConfirmed = false;
                emailResponse.Message = "User not found";
                return emailResponse;
            }

            var result = await _unitOfWork.Users.UserManager.ConfirmEmailAsync(user, emailRequest.Token);
            if (result.Succeeded)
            {
                emailResponse.IsConfirmed = true;
                emailResponse.Message = "Email confirmed successfully!";
                return emailResponse;
            }

            emailResponse.IsConfirmed = false;
            emailResponse.Message = "The Token is incorrect or expired";
            return emailResponse;
        }

    }
}
