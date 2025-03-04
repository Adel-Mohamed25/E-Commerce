using Application.Features.V1.RoleFeatures.Queries.RoleQueries;
using Application.Helper.ResponseServices;
using AutoMapper;
using Infrastructure.UnitOfWorks;
using MediatR;
using Microsoft.Extensions.Logging;
using Models.ResponseModels;
using Models.Role;

namespace Application.Features.V1.RoleFeatures.Queries.RoleQueriesHandler
{
    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, Response<IEnumerable<RoleModel>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllRolesQueryHandler> _logger;

        public GetAllRolesQueryHandler(IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<GetAllRolesQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Response<IEnumerable<RoleModel>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (!await _unitOfWork.Roles.IsExistAsync(cancellationToken: cancellationToken))
                    return ResponseHandler.NotFound<IEnumerable<RoleModel>>(message: "No any Roles");
                var roles = _unitOfWork.Roles.RoleManager.Roles;
                var result = _mapper.Map<IEnumerable<RoleModel>>(roles);
                return ResponseHandler.Success(data: result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return ResponseHandler.BadRequest<IEnumerable<RoleModel>>();
            }
        }
    }
}
