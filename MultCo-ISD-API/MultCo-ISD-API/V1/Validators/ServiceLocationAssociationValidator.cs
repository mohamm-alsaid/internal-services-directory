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
            RuleFor(x => x.ServiceLocationAssociation1)
                .NotNull().WithMessage("Service Location Association 1 cannot be null")
                .GreaterThan(0).WithMessage("Service Location Association 1 cannot be 0");

            RuleFor(x => x.ServiceId)
                .NotNull().WithMessage("Service id cannot be null")
                .GreaterThan(0).WithMessage("Service id cannot be 0");
            
            RuleFor(x => x.LocationId)
                .NotNull().WithMessage("Location id cannot be null")
                .GreaterThan(0).WithMessage("Location id cannot be 0");
        }
    }
}