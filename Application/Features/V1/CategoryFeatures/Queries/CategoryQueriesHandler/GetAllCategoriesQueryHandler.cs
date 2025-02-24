using Application.Features.V1.CategoryFeatures.Queries.CategoryQueries;
using Application.Helper.ResponseServices;
using AutoMapper;
using Infrastructure.UnitOfWorks;
using Infrastructure.Utilities.Caching.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Models.Category;
using Models.ResponseModels;
using System.Net;
using System.Net.Sockets;

namespace Application.Features.V1.CategoryFeatures.Queries.CategoryQueriesHandler
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, Response<IEnumerable<CategoryModel>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<GetAllCategoriesQueryHandler> _logger;
        private readonly IRedisCacheService _cache;

        public GetAllCategoriesQueryHandler(IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<GetAllCategoriesQueryHandler> logger
            , IRedisCacheService cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _cache = cache;
        }
        public async Task<Response<IEnumerable<CategoryModel>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                #region Get IP address from HttpContext
                var remoteIpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress;

                if (remoteIpAddress != null && remoteIpAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    _logger.LogInformation($"Client IPv4 Address: {remoteIpAddress}");
                }
                else
                {
                    _logger.LogWarning("IPv4 Address Not Found.");
                }
                #endregion

                #region Get host details
                var hostName = Dns.GetHostName();
                var hostEntry = await Dns.GetHostEntryAsync(hostName);
                _logger.LogInformation($"Host Name: {hostEntry.HostName}");
                #endregion

                if (!await _unitOfWork.Categories.IsExistAsync(cancellationToken: cancellationToken))
                    return ResponseHandler.NotFound<IEnumerable<CategoryModel>>();
                var categories = await _unitOfWork.Categories.GetAllAsync(orderBy: c => c.Name, cancellationToken: cancellationToken);
                var data = _mapper.Map<IEnumerable<CategoryModel>>(categories);
                var result = _cache.GetData<IEnumerable<CategoryModel>>("Categories");
                if (result != null)
                    return ResponseHandler.Success(result);
                _cache.SetData("Categories", data);
                return ResponseHandler.Success(data);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching categories.");
                return ResponseHandler.InternalServerError<IEnumerable<CategoryModel>>();
            }
        }
    }
}
