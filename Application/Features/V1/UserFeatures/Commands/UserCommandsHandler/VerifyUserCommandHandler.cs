using Application.Features.V1.UserFeatures.Commands.UserCommands;
using Application.Helper.ResponseServices;
using Infrastructure.UnitOfWorks;
using MediatR;
using Models.Email;
using Models.ResponseModels;
using Services.UnitOfServices;

namespace Application.Features.V1.UserFeatures.Commands.UserCommandsHandler
{
    public class VerifyUserCommandHandler : IRequestHandler<VerifyUserCommand, Response<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnitOfService _unitOfService;

        public VerifyUserCommandHandler(IUnitOfWork unitOfWork, IUnitOfService unitOfService)
        {
            _unitOfWork = unitOfWork;
            _unitOfService = unitOfService;
        }
        public async Task<Response<string>> Handle(VerifyUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.UserManager.FindByEmailAsync(request.VerifyEmail.Email);

            if (user is null)
                return ResponseHandler.NotFound<string>(message: "User Not Found");

            var code = await _unitOfService.AuthServices.GenerateVerificationCodeAsync(user);
            await _unitOfService.EmailServices.SendEmailAsync(new SendEmailModel
            {
                To = request.VerifyEmail.Email,
                Body = $"Dear Customer,your Verification Code is {code}." +
                "Please treat it as confidential and don’t share it with anyone. " +
                "WE will not ask you for this code." +
                "This code is valid for 5 minutes.",
                Subject = "Email verification code"
            });

            return ResponseHandler.Success(code);
        }
    }
}
