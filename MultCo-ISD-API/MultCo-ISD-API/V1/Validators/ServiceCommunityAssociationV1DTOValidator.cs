using FluentValidation;
using MultCo_ISD_API.V1.DTO;


namespace MultCo_ISD_API.V1.Validators
{
    public class ServiceCommunityAssociationV1DTOValidator : AbstractValidator<ServiceCommunityAssociationV1DTO>
    {
        public ServiceCommunityAssociationV1DTOValidator()
        {
            RuleFor(x => x.ServiceCommunityAssociationID)
               .Empty().WithMessage("cannot specify service community association ID explicitly");

            RuleFor(x => x.CommunityID)
                .NotNull().WithMessage("Community Id cannot be null")
                .GreaterThan(0).WithMessage("Community Id cannot be 0");

            RuleFor(x => x.ServiceID)
                .NotNull().WithMessage("Service Id cannot be null")
                .GreaterThan(0).WithMessage("Service Id cannot be 0");
                
        }
    }
}
