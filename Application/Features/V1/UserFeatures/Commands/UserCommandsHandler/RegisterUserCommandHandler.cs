using Application.Features.V1.UserFeatures.Commands.UserCommands;
using Application.Helper.ResponseServices;
using AutoMapper;
using Domain.Entities.Identity;
using Infrastructure.UnitOfWorks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RegisterUserCommandHandler(IUnitOfWork unitOfWork,
            IUnitOfService unitOfService,
            IMapper mapper,
            ILogger<RegisterUserCommandHandler> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _unitOfService = unitOfService;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
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

                    var url = $"{_httpContextAccessor.HttpContext.Request.Scheme.Trim().ToLower()}://{_httpContextAccessor.HttpContext.Request.Host.ToUriComponent().Trim().ToLower()}/api/v1/Email/ConfirmEmail";

                    var parameters = new Dictionary<string, string>
                    {
                        {"Token", token },
                        {"UserId", user.Id}
                    };

                    var confirmationLink = new Uri(QueryHelpers.AddQueryString(url, parameters));

                    var sendEmailModel = new SendEmailModel
                    {
                        To = user.Email!,
                        Subject = "Confirm Your Email",
                        Body = confirmationLink.ToString()
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
