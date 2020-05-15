using FluentValidation;
using MultCo_ISD_API.Models;
using MultCo_ISD_API.V1.DTO;

namespace MultCo_ISD_API.V1.Validators
{
    public class DivisionV1DTOValidator : AbstractValidator<DivisionV1DTO>
    {
        public DivisionV1DTOValidator()
        {
            RuleFor(x => x.DivisionId)
                .Empty().WithMessage("cannot specify division ID explicitly");

            RuleFor(x => x.DivisionCode)
                .NotNull().WithMessage("Division code cannot be null");

            RuleFor(x => x.DivisionName)
               .MaximumLength(255).WithMessage("Division name cannot exceed 255 characters");

        }
    }
}
