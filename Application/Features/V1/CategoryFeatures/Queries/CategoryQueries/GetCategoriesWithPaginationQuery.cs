using Domain.Enums;
using MediatR;
using Models.Category;
using Models.ResponseModels;

namespace Application.Features.V1.CategoryFeatures.Queries.CategoryQueries
{
    public record GetCategoriesWithPaginationQuery(int? pageNumber,
        int? pageSize,
        OrderBy orderBy,
        OrderByDirection orderByDirection) : IRequest<PaginationResponse<IEnumerable<CategoryModel>>>;
}
