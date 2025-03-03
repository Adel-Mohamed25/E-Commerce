using Application.Features.V1.RoleFeatures.Comands.RoleCommands;
using MediatR;
using Models.ResponseModels;
using Models.Role;

namespace Application.Features.V1.RoleFeatures.Comands.RoleCommandsHandler
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Response<RoleModel>>
    {
        public Task<Response<RoleModel>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

