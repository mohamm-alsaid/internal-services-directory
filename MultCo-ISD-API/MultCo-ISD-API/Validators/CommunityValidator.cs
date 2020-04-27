using FluentValidation;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.Validators
{
    public class CommunityValidator : AbstractValidator<Community>
    {
        public CommunityValidator()
        {
            RuleFor(x => x.CommunityId).NotNull();
            RuleFor(x => x.CommunityName).NotNull().MaximumLength(50);
            RuleFor(x => x.CommunityDescription).NotNull().MaximumLength(50);
        }
    }
}
