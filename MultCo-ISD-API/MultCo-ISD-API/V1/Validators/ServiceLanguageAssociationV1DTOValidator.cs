using System.Data;
using FluentValidation;
using FluentValidation.Validators;
using MultCo_ISD_API.V1.DTO;

namespace MultCo_ISD_API.V1.Validators
{
    public class ServiceLanguageAssociationValidator : AbstractValidator<ServiceLanguageAssociationV1DTO>
    {
        public ServiceLanguageAssociationValidator()
        {
            RuleFor(x => x.ServiceID).NotNull();
            RuleFor(x => x.LanguageID).NotNull();
        }
    }
}
