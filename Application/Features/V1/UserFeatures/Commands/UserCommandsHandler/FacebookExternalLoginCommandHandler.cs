using Application.Features.V1.UserFeatures.Commands.UserCommands;
using MediatR;
using Models.Authentication;
using Models.ResponseModels;

namespace Application.Features.V1.UserFeatures.Commands.UserCommandsHandler
{
    public class FacebookExternalLoginCommandHandler : IRequestHandler<FacebookExternalLoginCommand, Response<AuthModel>>
    {
        public Task<Response<AuthModel>> Handle(FacebookExternalLoginCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
