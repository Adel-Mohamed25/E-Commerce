using MediatR;
using Models.Category;
using Models.ResponseModels;

namespace Application.Features.CategoryFeatures.Queries.CategoryQueries
{
    public record GetAllCategoriesQuery() : IRequest<Response<IEnumerable<CategoryModel>>>;
}
