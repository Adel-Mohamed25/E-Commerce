using MediatR;
using Models.Authentication;
using Models.ResponseModels;

namespace Application.Features.UserFeatures.Commands.UserCommands
{
    public record LoginUserCommand(LoginModel loginModel) : IRequest<Response<AuthModel>>;
}
