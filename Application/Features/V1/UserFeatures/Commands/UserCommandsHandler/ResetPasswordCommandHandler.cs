using Application.Features.V1.UserFeatures.Commands.UserCommands;
using Application.Helper.ResponseServices;
using Infrastructure.UnitOfWorks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Models.ResponseModels;
using Services.UnitOfServices;

namespace Application.Features.V1.UserFeatures.Commands.UserCommandsHandler
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Response<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnitOfService _unitOfService;

        public ResetPasswordCommandHandler(IUnitOfWork unitOfWork, IUnitOfService unitOfService)
        {
            _unitOfWork = unitOfWork;
            _unitOfService = unitOfService;
        }

        public async Task<Response<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.UserManager.FindByEmailAsync(request.ResetPassword.Email);
            if (user is null)
                return ResponseHandler.NotFound<string>(message: "User Not Found");

            var isvaild = await _unitOfService.AuthServices.VerifyCodeAsync(user, request.ResetPassword.Code);
            if (!isvaild)
                return ResponseHandler.BadRequest<string>(message: "verification code Not Vaild");

            var token = await _unitOfWork.Users.UserManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult result = await _unitOfWork.Users.UserManager.ResetPasswordAsync(user, token, request.ResetPassword.Password);
            if (result.Succeeded)
                return ResponseHandler.Success<string>(message: "Password changed successfully");
            return ResponseHandler.Conflict<string>();
        }
    }
}
