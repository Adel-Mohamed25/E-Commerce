using MediatR;
using Models.Category;
using Models.ResponseModels;

namespace Application.Features.CategoryFeatures.Commands.CategoryCommands
{
    public record PostCategoryCommand(PostCategoryModel PostCategoryModel) : IRequest<Response<PostCategoryModel>>;
}
