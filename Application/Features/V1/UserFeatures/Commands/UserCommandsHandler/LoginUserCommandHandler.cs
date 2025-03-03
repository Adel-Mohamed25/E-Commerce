using Application.Features.V1.UserFeatures.Commands.UserCommands;
using Application.Helper.ResponseServices;
using Infrastructure.UnitOfWorks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Models.Authentication;
using Models.ResponseModels;
using Services.UnitOfServices;
using System.Net;
using System.Net.Sockets;

namespace Application.Features.V1.UserFeatures.Commands.UserCommandsHandler
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Response<AuthModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnitOfService _unitOfService;
        private readonly ILogger<LoginUserCommandHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginUserCommandHandler(IUnitOfWork unitOfWork
            , IUnitOfService unitOfService
            , ILogger<LoginUserCommandHandler> logger
            , IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _unitOfService = unitOfService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Response<AuthModel>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                #region Get IP address from HttpContext
                var remoteIpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress;

                if (remoteIpAddress != null && remoteIpAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    _logger.LogInformation($"Client IPv4 Address: {remoteIpAddress}");
                }
                else
                {
                    _logger.LogWarning("IPv4 Address Not Found.");
                }
                #endregion

                #region Get host details
                var hostName = Dns.GetHostName();
                var hostEntry = await Dns.GetHostEntryAsync(hostName);
                _logger.LogInformation($"Host Name: {hostEntry.HostName}");
                #endregion

                var findUser = await _unitOfWork.Users.UserManager.FindByEmailAsync(request.loginModel.Email);
                if (findUser == null)
                    return ResponseHandler.NotFound<AuthModel>(errors: "User not found");

                var user = await _unitOfWork.Users.GetByAsync(
                    mandatoryFilter: u => u.Id == findUser.Id,
                    cancellationToken: cancellationToken,
                    includes: $"{nameof(findUser.JwtTokens)}"
                );

                bool isPasswordCorrect = await _unitOfWork.Users.UserManager.CheckPasswordAsync(findUser, request.loginModel.Password);

                if (!isPasswordCorrect)
                    return ResponseHandler.BadRequest<AuthModel>(errors: "Incorrect email or password");



                var authModel = await _unitOfService.AuthServices.GetTokenAsync(user);
                return ResponseHandler.Success(authModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return ResponseHandler.InternalServerError<AuthModel>(errors: "An error occurred while processing the request");
            }
        }
    }
}
