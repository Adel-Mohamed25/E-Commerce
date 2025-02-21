using MediatR;
using Models.Category;
using Models.ResponseModels;

namespace Application.Features.V1.CategoryFeatures.Queries.CategoryQueries
{
    public record GetAllCategoriesQuery() : IRequest<Response<IEnumerable<CategoryModel>>>;
}
