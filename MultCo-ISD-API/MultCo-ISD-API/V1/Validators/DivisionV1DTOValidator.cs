using FluentValidation;
using MultCo_ISD_API.V1.DTO;

namespace MultCo_ISD_API.V1.Validators
{
    public class DivisionV1DTOValidator : AbstractValidator<DivisionV1DTO>
    {
        public DivisionV1DTOValidator()
        {
            RuleFor(x => x.DivisionID).NotNull();
            RuleFor(x => x.DivisionCode).NotNull();
            RuleFor(x => x.DivisionName).NotNull().MaximumLength(20);

        }
    }
}
