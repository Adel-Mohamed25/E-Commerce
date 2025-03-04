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
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Response<RoleModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateRoleCommandHandler> _logger;

        public UpdateRoleCommandHandler(IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<UpdateRoleCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<RoleModel>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id != request.RoleModel.Id)
                    return ResponseHandler.BadRequest<RoleModel>(message: "Different identifier in both cases");

                var role = await _unitOfWork.Roles.RoleManager.FindByIdAsync(request.Id);
                if (role == null)
                    return ResponseHandler.NotFound<RoleModel>(message: "Role Not Found");

                role.ModifiedDate = DateTime.UtcNow;
                _mapper.Map(request.RoleModel, role);
                await _unitOfWork.Roles.RoleManager.UpdateAsync(role);
                return ResponseHandler.Success(data: request.RoleModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return ResponseHandler.BadRequest<RoleModel>();
            }
        }
    }
}

