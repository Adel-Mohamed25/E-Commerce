using MediatR;
using Models.ResponseModels;

namespace Application.Features.V1.UserFeatures.Commands.UserCommands
{
    public record LogoutUserCommand() : IRequest<Response<string>>;
}
