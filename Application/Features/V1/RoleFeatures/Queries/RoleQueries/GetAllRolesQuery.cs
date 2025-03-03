using MediatR;
using Models.ResponseModels;
using Models.Role;

namespace Application.Features.V1.RoleFeatures.Queries.RoleQueries
{
    public record GetAllRolesQuery() : IRequest<Response<IEnumerable<RoleModel>>>;
}
