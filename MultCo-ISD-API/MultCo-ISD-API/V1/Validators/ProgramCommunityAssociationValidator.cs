using System.Data;
using FluentValidation;
using FluentValidation.Validators;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.V1.Validators
{
    public class ProgramConmunnityAssociationValidator : AbstractValidator<ProgramCommunityAssociation>
    {
        public ProgramConmunnityAssociationValidator()
        {
            RuleFor(x => x.ProgramCommunityAssociationId);
            //
            RuleFor(x => x.CommunityId);
            RuleFor(x => x.ServiceId);
            //
        }
    }
}
