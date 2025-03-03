using Application.Features.V1.RoleFeatures.Queries.RoleQueries;
using MediatR;
using Models.ResponseModels;
using Models.Role;

namespace Application.Features.V1.RoleFeatures.Queries.RoleQueriesHandler
{
    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, Response<RoleModel>>
    {
        public Task<Response<RoleModel>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
