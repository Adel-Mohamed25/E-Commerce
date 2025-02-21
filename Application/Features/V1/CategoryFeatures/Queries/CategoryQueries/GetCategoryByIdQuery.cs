using MediatR;
using Models.Category;
using Models.ResponseModels;

namespace Application.Features.V1.CategoryFeatures.Queries.CategoryQueries
{
    public record GetCategoryByIdQuery(string Id) : IRequest<Response<CategoryModel>>;
}
