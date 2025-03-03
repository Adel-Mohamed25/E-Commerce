using Application.Features.V1.RoleFeatures.Queries.RoleQueries;
using MediatR;
using Models.ResponseModels;
using Models.Role;

namespace Application.Features.V1.RoleFeatures.Queries.RoleQueriesHandler
{
    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, Response<IEnumerable<RoleModel>>>
    {
        public Task<Response<IEnumerable<RoleModel>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
