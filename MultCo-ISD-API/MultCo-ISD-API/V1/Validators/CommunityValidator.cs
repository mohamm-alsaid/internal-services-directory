using FluentValidation;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.V1.Validators
{
    public class CommunityValidator : AbstractValidator<Community>
    {
        public CommunityValidator()
        {
            //RuleFor(x => x.CommunityId).NotNull().WithMessage("Community I.D cannot be null").NotEqual(0).WithMessage("Community Id should start from 1");
            RuleFor(x => x.CommunityName).NotNull().WithMessage("Community name cannot be null")
                .MaximumLength(50).WithMessage("Community name cannot exceed 50 chars");
            RuleFor(x => x.CommunityDescription).NotNull().MaximumLength(50);
        }
    }
}
