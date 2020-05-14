using FluentValidation;
using MultCo_ISD_API.V1.DTO;

namespace MultCo_ISD_API.V1.Validators
{
    public class CommunityV1DTOValidator : AbstractValidator<CommunityV1DTO>
    {
        public CommunityV1DTOValidator()
        {
            RuleFor(x => x.CommunityID)
               .Empty().WithMessage("cannot specify community ID explicitly");

            RuleFor(x => x.CommunityName)
                .NotNull().WithMessage("Community name cannot be null")
                .MaximumLength(50).WithMessage("Community name cannot exceed 50 chars");

            RuleFor(x => x.CommunityDescription)
                .NotNull().WithMessage("Community Description cannot be null")
                .MaximumLength(50).WithMessage("Community Description cannot exceed 50 chars");
        }
    }
}
