using MediatR;
using Models.Category;
using Models.ResponseModels;

namespace Application.Features.CategoryFeatures.Queries.CategoryQueries
{
    public record GetCategoryByIdQuery(string Id) : IRequest<Response<CategoryModel>>;
}
