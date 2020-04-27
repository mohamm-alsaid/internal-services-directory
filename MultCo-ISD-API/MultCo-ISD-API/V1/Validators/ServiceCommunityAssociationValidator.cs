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
            RuleFor(x => x.ServiceCommunityAssociationId);
            //
            RuleFor(x => x.CommunityId);
            RuleFor(x => x.ServiceId);
            //
        }
    }
}
