using System.Data;
using FluentValidation;
using FluentValidation.Validators;
using MultCo_ISD_API.Models;
using MultCo_ISD_API.V1.DTO;


namespace MultCo_ISD_API.V1.Validators
{
    public class ServiceLocationAssociationV1DTOValidator : AbstractValidator<ServiceLocationAssociationV1DTO>
    {
        public ServiceLocationAssociationV1DTOValidator()
        {
            RuleFor(x => x.ServiceLocationAssociation)
               .Empty().WithMessage("cannot specify service location ID explicitly");

            RuleFor(x => x.ServiceId)
                .NotNull().WithMessage("Service id cannot be null")
                .GreaterThan(0).WithMessage("Service id cannot be 0");
            
            RuleFor(x => x.LocationId)
                .NotNull().WithMessage("Location id cannot be null")
                .GreaterThan(0).WithMessage("Location id cannot be 0");
                
        }
    }
}