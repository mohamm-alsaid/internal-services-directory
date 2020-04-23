using System.Data;
using FluentValidation;
using FluentValidation.Validators;
using MultCo_ISD_API.V1.DTO;

namespace MultCo_ISD_API.V1.Validators
{
    public class ProgramConmunnityAssociationV1DTOValidator : AbstractValidator<ProgramCommunityAssociationV1DTO>
    {
        public ProgramConmunnityAssociationV1DTOValidator()
        {
            RuleFor(x => x.ProgramCommunityAssociationID);
            //
            RuleFor(x => x.CommunityID);
            RuleFor(x => x.ServiceID);
            //
        }
    }
}
