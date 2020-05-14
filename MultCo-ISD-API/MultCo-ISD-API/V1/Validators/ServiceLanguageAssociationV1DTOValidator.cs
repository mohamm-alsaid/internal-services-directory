using System.Data;
using FluentValidation;
using FluentValidation.Validators;
using MultCo_ISD_API.Models;
using MultCo_ISD_API.V1.DTO;


namespace MultCo_ISD_API.V1.Validators
{
    public class ServiceLanguageAssociationV1DTOValidator : AbstractValidator<ServiceLanguageAssociationV1DTO>
    {
        public ServiceLanguageAssociationV1DTOValidator()
        {
            RuleFor(x => x.ServiceLanguageAssociation)
               .Empty().WithMessage("cannot specify service language ID explicitly");

            RuleFor(x => x.ServiceID)
                .NotNull().WithMessage("Service Id cannot be null")
                .GreaterThan(0).WithMessage("Service Id cannot be 0");
            
            RuleFor(x => x.LanguageID)
                .NotNull().WithMessage("Language Id cannot be null")
                .GreaterThan(0).WithMessage("Language Id cannot be 0");
                
        }
    }
}
