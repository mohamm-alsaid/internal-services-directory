using System.Data;
using FluentValidation;
using FluentValidation.Validators;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.V1.Validators
{
    public class ServiceLanguageAssociationValidator : AbstractValidator<ServiceLanguageAssociation>
    {
        public ServiceLanguageAssociationValidator()
        {
            RuleFor(x => x.ServiceLanguageAssociation1)
                .NotNull().WithMessage("Service Language Association 1 cannot be null")
                .GreaterThan(0).WithMessage("Service Language Association 1 cannot be 0");

            RuleFor(x => x.ServiceId)
                .NotNull().WithMessage("Service Id cannot be null")
                .GreaterThan(0).WithMessage("Service Id cannot be 0");
            
            RuleFor(x => x.LanguageId)
                .NotNull().WithMessage("Language Id cannot be null")
                .GreaterThan(0).WithMessage("Language Id cannot be 0");
        }
    }
}
