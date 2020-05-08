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
            RuleFor(x => x.ProgramID)
                .Empty().WithMessage("cannot specify program ID explicitly");

            RuleFor(x => x.SponsorName)
                .NotNull().WithMessage("sponsor name cannot be null")
                .MaximumLength(20).WithMessage("sponsor name cannot exceed 20 characters");

            RuleFor(x => x.ProgramName)
                .NotNull().WithMessage("program name cannot be null")
                .MaximumLength(30).WithMessage("program name exceed 30");

            RuleFor(x => x.OfferType)
                .NotNull().WithMessage("program offer type cannot be null")
                .MaximumLength(20).WithMessage("program offer type cannot exceed 20");

            RuleFor(x => x.ProgramOfferNumber).NotNull().WithMessage("program offer number cannot be null")
                .MaximumLength(10).WithMessage("program offer number cannot exceed 10");
        }
    }
}
