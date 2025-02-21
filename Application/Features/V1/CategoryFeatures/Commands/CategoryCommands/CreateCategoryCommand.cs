using MediatR;
using Models.Category;
using Models.ResponseModels;

namespace Application.Features.V1.CategoryFeatures.Commands.CategoryCommands
{
    public record CreateCategoryCommand(CreateCategoryModel CreateCategoryModel) : IRequest<Response<CreateCategoryModel>>;
}
