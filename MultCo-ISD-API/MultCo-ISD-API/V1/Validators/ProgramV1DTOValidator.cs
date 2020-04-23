using System.Data;
using FluentValidation;
using FluentValidation.Validators;
using MultCo_ISD_API.V1.DTO;

namespace MultCo_ISD_API.V1.Validators
{
    public class ProgramV1DTOValidator : AbstractValidator<ProgramV1DTO>
    {
        public ProgramV1DTOValidator()
        {
            RuleFor(x => x.ProgramID).NotNull();
            RuleFor(x => x.SponsorName).NotNull().MaximumLength(20);
            RuleFor(x => x.OfferType).NotNull().MaximumLength(20);
        }
    }
}
