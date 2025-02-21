using Application.Features.V1.CategoryFeatures.Queries.CategoryQueries;
using FluentValidation;

namespace Application.Features.V1.CategoryFeatures.Validations.CategoryQueriesValidation
{
    public class GetCategoryByIdQueryValidation : AbstractValidator<GetCategoryByIdQuery>
    {
        public GetCategoryByIdQueryValidation()
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

        }
    }
}
