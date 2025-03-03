using Application.Features.V1.CategoryFeatures.Queries.CategoryQueries;
using Application.Helper.ResponseServices;
using Application.Resources;
using AutoMapper;
using Infrastructure.UnitOfWorks;
using Infrastructure.Utilities.Caching.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Models.Category;
using Models.ResponseModels;

namespace Application.Features.V1.CategoryFeatures.Queries.CategoryQueriesHandler
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, Response<IEnumerable<CategoryModel>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<GetAllCategoriesQueryHandler> _logger;
        private readonly IRedisCacheService _cache;
        private readonly IStringLocalizer<SharedResource> _stringLocalizer;

        public GetAllCategoriesQueryHandler(IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<GetAllCategoriesQueryHandler> logger
            , IRedisCacheService cache,
            IStringLocalizer<SharedResource> stringLocalizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _cache = cache;
            _stringLocalizer = stringLocalizer;
        }
        public async Task<Response<IEnumerable<CategoryModel>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null || httpContext.User.Identity?.IsAuthenticated == false)
                    return ResponseHandler.Unauthorized<IEnumerable<CategoryModel>>(message: "Unauthorized request");


                if (!await _unitOfWork.Categories.IsExistAsync(cancellationToken: cancellationToken))
                    return ResponseHandler.NotFound<IEnumerable<CategoryModel>>();
                var categories = await _unitOfWork.Categories.GetAllAsync(orderBy: c => c.Name, cancellationToken: cancellationToken);
                var data = _mapper.Map<IEnumerable<CategoryModel>>(categories);
                var result = _cache.GetData<IEnumerable<CategoryModel>>("Categories");
                if (result != null)
                    return ResponseHandler.Success(data: result, message: _stringLocalizer[SharedResourceKeys.SuccessMessage]);
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
