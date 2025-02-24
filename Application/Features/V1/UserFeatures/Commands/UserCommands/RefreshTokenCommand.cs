using MediatR;
using Models.Authentication;
using Models.ResponseModels;

namespace Application.Features.V1.UserFeatures.Commands.UserCommands
{
    public record RefreshTokenCommand(RefreshTokenRequestModel RefreshTokenRequest) : IRequest<Response<AuthModel>>;
}
