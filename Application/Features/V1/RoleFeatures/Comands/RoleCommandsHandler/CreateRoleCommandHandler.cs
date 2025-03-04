using Application.Features.V1.RoleFeatures.Comands.RoleCommands;
using Application.Helper.ResponseServices;
using AutoMapper;
using Domain.Entities.Identity;
using Infrastructure.UnitOfWorks;
using MediatR;
using Microsoft.Extensions.Logging;
using Models.ResponseModels;
using Models.Role;

namespace Application.Features.V1.RoleFeatures.Comands.RoleCommandsHandler
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Response<CreateRoleModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateRoleCommandHandler> _logger;

        public CreateRoleCommandHandler(IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<CreateRoleCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Response<CreateRoleModel>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var role = _mapper.Map<Role>(request.CreateRoleModel);
                role.CreatedDate = DateTime.UtcNow;
                await _unitOfWork.Roles.RoleManager.CreateAsync(role);
                return ResponseHandler.Success(request.CreateRoleModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during create role.");
                return ResponseHandler.Conflict<CreateRoleModel>();
            }
        }
    }
}
