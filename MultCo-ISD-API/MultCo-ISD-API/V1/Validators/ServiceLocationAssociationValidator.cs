using System.Data;
using FluentValidation;
using FluentValidation.Validators;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.V1.Validators
{
    public class ServiceLocationAssociationValidator : AbstractValidator<ServiceLocationAssociation>
    {
        public ServiceLocationAssociationValidator()
        {
            RuleFor(x => x.ServiceId).NotNull();
            RuleFor(x => x.LocationId).NotNull();
        }
    }
}