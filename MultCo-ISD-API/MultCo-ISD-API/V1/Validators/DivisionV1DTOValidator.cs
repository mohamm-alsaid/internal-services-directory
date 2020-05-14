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
                .NotNull().WithMessage("Division name cannot be null")
                .MaximumLength(30).WithMessage("Division name cannot exceed 30 characters");

        }
    }
}
