using FluentValidation;
using MultCo_ISD_API.V1.DTO;

namespace MultCo_ISD_API.V1.Validators
{
    public class CommunityV1DTOValidator : AbstractValidator<CommunityV1DTO>
    {
        public CommunityV1DTOValidator()
        {

            RuleFor(x => x.CommunityId)
               .Empty().WithMessage("cannot specify CommunityId explicitly");

            RuleFor(x => x.CommunityName)
                .MaximumLength(255).WithMessage("Community name cannot exceed 255 chars");

            RuleFor(x => x.CommunityDescription)
                .MaximumLength(255).WithMessage("Community Description cannot exceed 255 chars");
        }
    }
}
