using System.Data;
using FluentValidation;
using FluentValidation.Validators;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.V1.Validators
{
    public class ServiceLocationAssociationV1DTOValidator : AbstractValidator<ServiceLocationAssociation>
    {
        public ServiceLocationAssociationV1DTOValidator()
        {
            RuleFor(x => x.ServiceId).NotNull();
            RuleFor(x => x.LocationId).NotNull();
        }
    }
}