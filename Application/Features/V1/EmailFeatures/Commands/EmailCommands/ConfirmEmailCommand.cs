using MediatR;
using Models.Email;
using Models.ResponseModels;

namespace Application.Features.V1.EmailFeatures.Commands.EmailCommands
{
    public record ConfirmEmailCommand(EmailRequest EmailRequest) : IRequest<Response<EmailResponse>>;
}
