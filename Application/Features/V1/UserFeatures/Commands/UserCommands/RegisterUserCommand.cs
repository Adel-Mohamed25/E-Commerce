using MediatR;
using Models.Authentication;
using Models.ResponseModels;
using Models.User;

namespace Application.Features.V1.UserFeatures.Commands.UserCommands
{
    public record RegisterUserCommand(CreateUserModel PostUserModel) : IRequest<Response<AuthModel>>;
}
