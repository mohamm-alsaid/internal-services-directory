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
            RuleFor(x => x.ServiceLanguageAssociation1)
               .Empty().WithMessage("cannot specify service LanguageId explicitly");

            RuleFor(x => x.ServiceId)
                .NotNull().WithMessage("ServiceId cannot be null")
                .GreaterThan(0).WithMessage("ServiceId cannot be 0");
            
            RuleFor(x => x.LanguageId)
                .NotNull().WithMessage("LanguageId cannot be null")
                .GreaterThan(0).WithMessage("LanguageId cannot be 0");
                
        }
    }
}
