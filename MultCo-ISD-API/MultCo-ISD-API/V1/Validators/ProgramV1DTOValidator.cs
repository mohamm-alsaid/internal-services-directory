using System.Data;
using FluentValidation;
using FluentValidation.Validators;
using MultCo_ISD_API.Models;
using MultCo_ISD_API.V1.DTO;


namespace MultCo_ISD_API.V1.Validators
{
    public class ProgramV1DTOValidator : AbstractValidator<ProgramV1DTO>
    {
        public ProgramV1DTOValidator()
        {
            RuleFor(x => x.ProgramId)
                .Empty().WithMessage("cannot specify program ID explicitly");

            RuleFor(x => x.SponsorName)
                .MaximumLength(255).WithMessage("sponsor name cannot exceed 255 chars");

            RuleFor(x => x.ProgramName)
                .MaximumLength(255).WithMessage("program name exceed 255 chars");

            RuleFor(x => x.OfferType)
                .MaximumLength(255).WithMessage("program offer type cannot exceed 255 chars");

            RuleFor(x => x.ProgramOfferNumber)
                .MaximumLength(255).WithMessage("program offer number cannot exceed 255 chars");
        }
    }
}
