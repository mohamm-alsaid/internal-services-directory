using FluentValidation;
using MultCo_ISD_API.V1.DTO;

namespace MultCo_ISD_API.V1.Validators
{
    public class CommunityV1DTOValidator : AbstractValidator<CommunityV1DTO>
    {
        public CommunityV1DTOValidator()
        {
            RuleFor(x => x.CommunityName).NotNull();
            RuleFor(x => x.CommunityName).NotNull().MaximumLength(5);
            RuleFor(x => x.CommunityDescription).NotNull().MaximumLength(5);
        }
    }
}
