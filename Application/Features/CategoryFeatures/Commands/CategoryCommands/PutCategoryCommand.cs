using MediatR;
using Models.Category;
using Models.ResponseModels;

namespace Application.Features.CategoryFeatures.Commands.CategoryCommands
{
    public record PutCategoryCommand(string Id, CategoryModel CategoryModel) : IRequest<Response<CategoryModel>>;
}
