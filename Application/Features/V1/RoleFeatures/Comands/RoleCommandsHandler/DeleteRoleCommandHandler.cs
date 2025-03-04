using Application.Features.V1.RoleFeatures.Comands.RoleCommands;
using Application.Helper.ResponseServices;
using AutoMapper;
using Infrastructure.UnitOfWorks;
using MediatR;
using Microsoft.Extensions.Logging;
using Models.ResponseModels;
using Models.Role;

namespace Application.Features.V1.RoleFeatures.Comands.RoleCommandsHandler
{
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Response<RoleModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteRoleCommandHandler> _logger;

        public DeleteRoleCommandHandler(IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<DeleteRoleCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Response<RoleModel>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var role = await _unitOfWork.Roles.RoleManager.FindByIdAsync(request.Id);
                if (role == null)
                    return ResponseHandler.NotFound<RoleModel>(message: "Role Not Found");
                await _unitOfWork.Roles.RoleManager.DeleteAsync(role);
                var result = _mapper.Map<RoleModel>(role);
                return ResponseHandler.Success(data: result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return ResponseHandler.BadRequest<RoleModel>();
            }
        }
    }
}
