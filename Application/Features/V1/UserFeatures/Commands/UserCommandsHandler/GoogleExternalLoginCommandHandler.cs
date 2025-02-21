using Application.Features.V1.UserFeatures.Commands.UserCommands;
using MediatR;
using Models.Authentication;
using Models.ResponseModels;

namespace Application.Features.V1.UserFeatures.Commands.UserCommandsHandler
{
    public class GoogleExternalLoginCommandHandler : IRequestHandler<GoogleExternalLoginCommand, Response<AuthModel>>
    {
        public Task<Response<AuthModel>> Handle(GoogleExternalLoginCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
