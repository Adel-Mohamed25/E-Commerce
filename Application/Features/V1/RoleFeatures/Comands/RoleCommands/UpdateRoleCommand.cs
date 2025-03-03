using MediatR;
using Models.ResponseModels;
using Models.Role;

namespace Application.Features.V1.RoleFeatures.Comands.RoleCommands
{
    public record UpdateRoleCommand(string Id, RoleModel RoleModel) : IRequest<Response<RoleModel>>;
}
