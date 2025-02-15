using Application.Features.UserFeatures.Commands.UserCommands;
using Application.Helper.ResponseServices;
using Infrastructure.UnitOfWorks;
using MediatR;
using Models.Authentication;
using Models.ResponseModels;
using Services.UnitOfServices;

namespace Application.Features.UserFeatures.Commands.UserCommandsHandler
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Response<AuthModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnitOfService _unitOfService;

        public LoginUserCommandHandler(IUnitOfWork unitOfWork, IUnitOfService unitOfService)
        {
            _unitOfWork = unitOfWork;
            _unitOfService = unitOfService;
        }
        public async Task<Response<AuthModel>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
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
            catch (Exception)
            {
                return ResponseHandler.InternalServerError<AuthModel>(errors: "An error occurred while processing the request");
            }
        }
    }
}
