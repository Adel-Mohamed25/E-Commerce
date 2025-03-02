using Application.Features.V1.CategoryFeatures.Queries.CategoryQueries;
using Application.Helper.ResponseServices;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.UnitOfWorks;
using Infrastructure.Utilities.Caching.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Models.Category;
using Models.ResponseModels;
using System.Linq.Expressions;

namespace Application.Features.V1.CategoryFeatures.Queries.CategoryQueriesHandler
{
    public class GetCategoriesWithPaginationQueryHandler
        : IRequestHandler<GetCategoriesWithPaginationQuery,
            PaginationResponse<IEnumerable<CategoryModel>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRedisCacheService _cache;
        private readonly ILogger<GetCategoriesWithPaginationQueryHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetCategoriesWithPaginationQueryHandler(IUnitOfWork unitOfWork,
            IMapper mapper,
            IRedisCacheService cache,
            ILogger<GetCategoriesWithPaginationQueryHandler> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<PaginationResponse<IEnumerable<CategoryModel>>> Handle(GetCategoriesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null || httpContext.User.Identity?.IsAuthenticated == false)
                return PaginationResponseHandler.Unauthorized<IEnumerable<CategoryModel>>(message: "Unauthorized request");

            if (!await _unitOfWork.Categories.IsExistAsync(cancellationToken: cancellationToken))
                return PaginationResponseHandler.NotFound<IEnumerable<CategoryModel>>();

            Expression<Func<Category, object>> orderBy = request.orderBy switch
            {
                OrderBy.Name => c => c.Name,
                _ => c => c.Id
            };

            var categories = await _unitOfWork.Categories.GetAllAsync(orderBy: orderBy,
                paginationOn: true,
                orderByDirection: request.orderByDirection,
                pageNumber: request.pageNumber,
                pageSize: request.pageSize,
                cancellationToken: cancellationToken);

            var data = _mapper.Map<IEnumerable<CategoryModel>>(categories);

            return PaginationResponseHandler.Success(data: data,
                pageNumber: request.pageNumber!.Value,
                pageSize: request.pageSize!.Value,
                totalCount: await _unitOfWork.Categories.CountAsync(cancellationToken: cancellationToken));

        }
    }
}
