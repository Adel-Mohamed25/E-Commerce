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
    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, Response<RoleModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetRoleByIdQueryHandler> _logger;

        public GetRoleByIdQueryHandler(IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<GetRoleByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Response<RoleModel>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var role = await _unitOfWork.Roles.RoleManager.FindByIdAsync(request.Id);
                if (role == null)
                    return ResponseHandler.NotFound<RoleModel>(message: "Role Not Found");
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
