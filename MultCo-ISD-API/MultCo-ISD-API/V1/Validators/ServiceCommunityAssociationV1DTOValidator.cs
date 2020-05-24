using FluentValidation;
using MultCo_ISD_API.V1.DTO;


namespace MultCo_ISD_API.V1.Validators
{
    public class ServiceCommunityAssociationV1DTOValidator : AbstractValidator<ServiceCommunityAssociationV1DTO>
    {
        public ServiceCommunityAssociationV1DTOValidator()
        {
            RuleFor(x => x.ServiceCommunityAssociationId)
               .Empty().WithMessage("cannot specify service CommunityAssociationId explicitly");

            RuleFor(x => x.CommunityId)
                .NotNull().WithMessage("CommunityId cannot be null")
                .GreaterThan(0).WithMessage("CommunityId cannot be 0");

            RuleFor(x => x.ServiceId)
                .NotNull().WithMessage("ServiceId cannot be null")
                .GreaterThan(0).WithMessage("ServiceId cannot be 0");
                
        }
    }
}
