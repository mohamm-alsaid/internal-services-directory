using FluentValidation;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.V1.Validators
{
    public class DivisionValidator : AbstractValidator<Division>
    {
        public DivisionValidator()
        {
            RuleFor(x => x.DivisionId)
                .NotNull().WithMessage("Division id cannot be null");

            RuleFor(x => x.DivisionCode)
                .NotNull().WithMessage("Division code cannot be null");

            RuleFor(x => x.DivisionName)
                .NotNull().WithMessage("Division name cannot be null")
                .MaximumLength(20).WithMessage("Division name cannot be longer than 20");

        }
    }
}
