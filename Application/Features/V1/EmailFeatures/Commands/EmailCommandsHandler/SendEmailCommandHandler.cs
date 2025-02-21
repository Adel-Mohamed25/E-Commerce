using Application.Features.V1.EmailFeatures.Commands.EmailCommands;
using Application.Helper.ResponseServices;
using MediatR;
using Models.Email;
using Models.ResponseModels;
using Services.UnitOfServices;

namespace Application.Features.V1.EmailFeatures.Commands.EmailCommandsHandler
{
    public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand, Response<EmailModel>>
    {
        private readonly IUnitOfService _unitOfService;

        public SendEmailCommandHandler(IUnitOfService unitOfService)
        {
            _unitOfService = unitOfService;
        }
        public async Task<Response<EmailModel>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            var response = await _unitOfService.EmailServices.SendEmailAsync(request.SendEmail);
            if (response.IsSuccess)
                return ResponseHandler.Success(response);
            return ResponseHandler.Conflict(response);
        }
    }
}
