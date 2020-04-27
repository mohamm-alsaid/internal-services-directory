using FluentValidation;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.Validators
{
    public class DivisionValidator : AbstractValidator<Division>
    {
        public DivisionValidator()
        {
            RuleFor(x => x.DivisionId).NotNull();
            RuleFor(x => x.DivisionCode).NotNull();
            RuleFor(x => x.DivisionName).NotNull().MaximumLength(20).WithMessage("long Division name");

        }
    }
}
