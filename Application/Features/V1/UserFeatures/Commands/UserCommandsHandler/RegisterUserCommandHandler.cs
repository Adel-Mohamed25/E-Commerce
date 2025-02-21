using Application.Features.V1.UserFeatures.Commands.UserCommands;
using Application.Helper.ResponseServices;
using AutoMapper;
using Domain.Entities.Identity;
using Infrastructure.UnitOfWorks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Models.Authentication;
using Models.Email;
using Models.ResponseModels;
using Services.UnitOfServices;

namespace Application.Features.V1.UserFeatures.Commands.UserCommandsHandler
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Response<AuthModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnitOfService _unitOfService;
        private readonly IMapper _mapper;
        private readonly ILogger<RegisterUserCommandHandler> _logger;

        public RegisterUserCommandHandler(IUnitOfWork unitOfWork, IUnitOfService unitOfService, IMapper mapper, ILogger<RegisterUserCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _unitOfService = unitOfService;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Response<AuthModel>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = _mapper.Map<User>(request.CreateUserModel);
                user.UserName = request.CreateUserModel.Email;
                IdentityResult result = await _unitOfWork.Users.UserManager.CreateAsync(user, request.CreateUserModel.Password);
                if (result.Succeeded)
                {
                    var authModel = await _unitOfService.AuthServices.GetTokenAsync(user);
                    var token = await _unitOfWork.Users.UserManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = $"https://localhost:44303/api/v1/Category/ConfirmEmail?UserId={user.Id}&Token={Uri.EscapeDataString(token)}";
                    var sendEmailModel = new SendEmailModel
                    {
                        To = user.Email!,
                        Subject = "Confirm Your Email",
                        Body = confirmationLink
                    };

                    var emaiModel = await _unitOfService.EmailServices.SendEmailAsync(sendEmailModel);
                    if (emaiModel.IsSuccess)
                        return ResponseHandler.Success(authModel);
                    return ResponseHandler.BadRequest<AuthModel>(message: "Failed to send confirmation email");
                }
                return ResponseHandler.BadRequest<AuthModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during user registration");
                return ResponseHandler.Conflict<AuthModel>(errors: $"{ex.Message}");
            }

        }
    }
}
