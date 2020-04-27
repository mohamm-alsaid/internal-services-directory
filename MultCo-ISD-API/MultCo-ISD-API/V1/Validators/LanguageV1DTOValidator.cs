using FluentValidation;
using MultCo_ISD_API.V1.DTO;

namespace MultCo_ISD_API.V1.Validators
{
    public class LanguageValidator : AbstractValidator<LanguageV1DTO>
    {
        public LanguageValidator()
        {
            RuleFor(x => x.LanguageID).NotNull();

            RuleFor(x => x.LanguageName).NotNull().MaximumLength(10);

        }
    }
}
