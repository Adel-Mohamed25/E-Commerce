using Application.Features.CategoryFeatures.Commands.CategoryCommands;
using FluentValidation;

namespace Application.Features.CategoryFeatures.Validations.CategoryCommandsValidation
{
    public class PutCategoryCommandValidation : AbstractValidator<PutCategoryCommand>
    {
        public PutCategoryCommandValidation()
        {
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage(c => $"{nameof(c.Id)} can not be not empty")
                .NotNull().WithMessage(c => $"{nameof(c.Id)} can not be not null")
                .MaximumLength(36).WithMessage(c => $"{nameof(c.Id)} his length can not be bigger than 36")
                .MinimumLength(36).WithMessage(c => $"{nameof(c.Id)} his length can not be less than 36");


            RuleFor(c => c.CategoryModel.Name)
                .NotEmpty().WithMessage(
                    c => $"{nameof(c.CategoryModel.Name)} can not be not empty")
                .NotNull().WithMessage(
                    c => $"{nameof(c.CategoryModel.Name)} can not be not null")
                .MaximumLength(100).WithMessage(
                    c => $"{nameof(c.CategoryModel.Name)} his length can not be bigger than {100}")
                .MinimumLength(1).WithMessage(
                    c => $"{nameof(c.CategoryModel.Name)} his length can not be less than {1}");
        }
    }
}
