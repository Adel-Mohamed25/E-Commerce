using Application.Features.V1.RoleFeatures.Comands.RoleCommands;
using MediatR;
using Models.ResponseModels;
using Models.Role;

namespace Application.Features.V1.RoleFeatures.Comands.RoleCommandsHandler
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Response<CreateRoleModel>>
    {
        public Task<Response<CreateRoleModel>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
