using System.Data;
using FluentValidation;
using FluentValidation.Validators;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.Validators
{
    public class ServiceLanguageAssociationValidator : AbstractValidator<ServiceLanguageAssociation>
    {
        public ServiceLanguageAssociationValidator()
        {
            RuleFor(x => x.ServiceId).NotNull();
            RuleFor(x => x.LanguageId).NotNull();
        }
    }
}
