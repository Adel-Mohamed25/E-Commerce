using Application.Features.V1.CategoryFeatures.Commands.CategoryCommands;
using FluentValidation;
using Infrastructure.UnitOfWorks;
using Models.Category;

namespace Application.Features.V1.CategoryFeatures.Validations.CategoryCommandsValidation
{
    public class CreateCategoryCommandValidation : AbstractValidator<CreateCategoryCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategoryCommandValidation(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(c => c.CreateCategoryModel.Name)
                .NotEmpty().WithMessage(c => $"{nameof(c.CreateCategoryModel.Name)} can not be empty")
                .NotNull().WithMessage(c => $"{nameof(c.CreateCategoryModel.Name)} can not be null")
                .MaximumLength(100).WithMessage(c => $"{nameof(c.CreateCategoryModel.Name)} his length can not be bigger than {100}")
                .MinimumLength(1).WithMessage(c => $"{nameof(c.CreateCategoryModel.Name)} his length can not be less than {1}");

            RuleFor(c => c.CreateCategoryModel)
                .MustAsync(CategoryNameUnique)
                .WithMessage("An category with the same name already exists.");
        }

        private async Task<bool> CategoryNameUnique(CreateCategoryModel model, CancellationToken cancellationToken)
        {
            return !await _unitOfWork.Categories.IsExistAsync(c => c.Name == model.Name, cancellationToken);
        }

    }
}
