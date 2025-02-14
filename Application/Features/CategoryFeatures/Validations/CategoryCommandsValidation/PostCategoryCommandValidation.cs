using Application.Features.CategoryFeatures.Commands.CategoryCommands;
using FluentValidation;
using Infrastructure.UnitOfWorks;
using Models.Category;

namespace Application.Features.CategoryFeatures.Validations.CategoryCommandsValidation
{
    public class PostCategoryCommandValidation : AbstractValidator<PostCategoryCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostCategoryCommandValidation(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(c => c.PostCategoryModel.Name)
                .NotEmpty().WithMessage(c => $"{nameof(c.PostCategoryModel.Name)} can not be empty")
                .NotNull().WithMessage(c => $"{nameof(c.PostCategoryModel.Name)} can not be null")
                .MaximumLength(100).WithMessage(c => $"{nameof(c.PostCategoryModel.Name)} his length can not be bigger than {100}")
                .MinimumLength(1).WithMessage(c => $"{nameof(c.PostCategoryModel.Name)} his length can not be less than {1}");

            RuleFor(c => c.PostCategoryModel)
                .MustAsync(CategoryNameUnique)
                .WithMessage("An category with the same name already exists.");
        }

        private async Task<bool> CategoryNameUnique(PostCategoryModel model, CancellationToken cancellationToken)
        {
            return !(await _unitOfWork.Categories.IsExistAsync(c => c.Name == model.Name, cancellationToken));
        }

    }
}
