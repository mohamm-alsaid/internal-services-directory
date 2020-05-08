using System.Data;
using FluentValidation;
using FluentValidation.Validators;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.V1.Validators
{
    public class ProgramValidator : AbstractValidator<Program>
    {
        public ProgramValidator()
        {
            RuleFor(x => x.SponsorName).NotNull().WithMessage("sponsor name cannot be null")
                .MaximumLength(20).WithMessage("sponsor name too long");

            RuleFor(x => x.OfferType).NotNull().WithMessage("program offer type cannot be null")
                .MaximumLength(20).WithMessage("program offer type too long");

            RuleFor(x => x.ProgramName).NotNull().WithMessage("program name cannot be null")
                .MaximumLength(20).WithMessage("program name too long");

            RuleFor(x => x.ProgramOfferNumber).NotNull().WithMessage("program offer number cannot be null")
                .MaximumLength(5).WithMessage("program offer number too long");
        }
    }
}
