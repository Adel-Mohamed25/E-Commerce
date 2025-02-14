using MediatR;
using Models.Category;
using Models.ResponseModels;

namespace Application.Features.CategoryFeatures.Commands.CategoryCommands
{
    public record DeleteCategoryCommand(string Id) : IRequest<Response<CategoryModel>>;
}
