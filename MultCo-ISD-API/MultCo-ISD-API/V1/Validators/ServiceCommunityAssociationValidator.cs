using System.Data;
using FluentValidation;
using FluentValidation.Validators;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.V1.Validators
{
    public class ServiceCommunityAssociationValidator : AbstractValidator<ServiceCommunityAssociation>
    {
        public ServiceCommunityAssociationValidator()
        {
            RuleFor(x => x.ServiceCommunityAssociationId)
                .NotNull().WithMessage("Service Community Association Id cannot be null")
                .GreaterThan(0).WithMessage("Service Community Association Id cannot be 0");

            RuleFor(x => x.CommunityId)
                .NotNull().WithMessage("Community Id cannot be null")
                .GreaterThan(0).WithMessage("Community Id cannot be 0");

            RuleFor(x => x.ServiceId)
                .NotNull().WithMessage("Service Id cannot be null")
                .GreaterThan(0).WithMessage("Service Id cannot be 0");
        }
    }
}
