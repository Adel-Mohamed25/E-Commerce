using Application.Features.V1.EmailFeatures.Commands.EmailCommands;
using Application.Helper.ResponseServices;
using MediatR;
using Models.Email;
using Models.ResponseModels;
using Services.UnitOfServices;

namespace Application.Features.V1.EmailFeatures.Commands.EmailCommandsHandler
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Response<EmailResponse>>
    {
        private readonly IUnitOfService _unitOfService;

        public ConfirmEmailCommandHandler(IUnitOfService unitOfService)
        {
            _unitOfService = unitOfService;
        }
        public async Task<Response<EmailResponse>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var response = await _unitOfService.EmailServices.ConfirmEmailAsync(request.EmailRequest);
            if (response.IsConfirmed)
                return ResponseHandler.Success(response);
            return ResponseHandler.Conflict(response);
        }
    }
}
