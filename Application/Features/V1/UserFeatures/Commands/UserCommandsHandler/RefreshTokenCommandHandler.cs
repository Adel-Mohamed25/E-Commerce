using Application.Features.V1.UserFeatures.Commands.UserCommands;
using Application.Helper.ResponseServices;
using Infrastructure.UnitOfWorks;
using MediatR;
using Models.Authentication;
using Models.ResponseModels;
using Services.UnitOfServices;

namespace Application.Features.V1.UserFeatures.Commands.UserCommandsHandler
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Response<AuthModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnitOfService _unitOfService;

        public RefreshTokenCommandHandler(IUnitOfWork unitOfWork, IUnitOfService unitOfService)
        {
            _unitOfWork = unitOfWork;
            _unitOfService = unitOfService;
        }
        public async Task<Response<AuthModel>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var jwtToken = await _unitOfWork.JwtTokens.GetByAsync(jt =>
                           jt.Token == request.RefreshTokenRequest.Token
                           && jt.RefreshToken == request.RefreshTokenRequest.RefreshToken
                           && jt.IsRefreshTokenUsed
                           , includes: "User");

            if (jwtToken == null)
                return ResponseHandler.NotFound<AuthModel>(message: "Token and RefreshToken Not Found");

            var jwtSecurityToken = await _unitOfService.AuthServices.ReadTokenAsync(request.RefreshTokenRequest.Token);
            if (jwtSecurityToken == null)
                return ResponseHandler.Unauthorized<AuthModel>(message: "Invalid JWT Token");

            bool isValid = await _unitOfService.AuthServices.IsTokenValidAsync(request.RefreshTokenRequest.Token, jwtSecurityToken);

            if (!isValid)
                return ResponseHandler.Unauthorized<AuthModel>(message: "Token and RefreshToken Not valid");

            var refreshToken = await _unitOfService.AuthServices.GetRefreshTokenAsync(jwtToken.User);
            return ResponseHandler.Success(refreshToken);

        }
    }
}
