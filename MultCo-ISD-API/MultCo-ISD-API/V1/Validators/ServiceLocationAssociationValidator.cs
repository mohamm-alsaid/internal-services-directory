using System.Data;
using FluentValidation;
using FluentValidation.Validators;
using MultCo_ISD_API.V1.DTO;

namespace MultCo_ISD_API.V1.Validators
{
    public class ServiceLocationAssociationV1DTOValidator : AbstractValidator<ServiceLocationAssociationV1DTO>
    {
        public ServiceLocationAssociationV1DTOValidator()
        {
            RuleFor(x => x.ServiceID).NotNull();
            RuleFor(x => x.LocationID).NotNull();
        }
    }
}