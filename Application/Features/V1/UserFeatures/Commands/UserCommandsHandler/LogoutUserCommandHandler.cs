using Application.Features.V1.UserFeatures.Commands.UserCommands;
using Application.Helper.ResponseServices;
using Infrastructure.UnitOfWorks;
using MediatR;
using Microsoft.Extensions.Logging;
using Models.ResponseModels;

namespace Application.Features.V1.UserFeatures.Commands.UserCommandsHandler
{
    public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand, Response<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LogoutUserCommandHandler> _logger;

        public LogoutUserCommandHandler(IUnitOfWork unitOfWork, ILogger<LogoutUserCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Response<string>> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.Users.SignInManager.SignOutAsync();

                return ResponseHandler.Success<string>(message: "Logout has been successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Logout failed");
                return ResponseHandler.BadRequest<string>();
            }
        }
    }
}
