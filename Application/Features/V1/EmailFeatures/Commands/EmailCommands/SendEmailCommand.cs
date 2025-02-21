using MediatR;
using Models.Email;
using Models.ResponseModels;

namespace Application.Features.V1.EmailFeatures.Commands.EmailCommands
{
    public record SendEmailCommand(SendEmailModel SendEmail) : IRequest<Response<EmailModel>>;
}
