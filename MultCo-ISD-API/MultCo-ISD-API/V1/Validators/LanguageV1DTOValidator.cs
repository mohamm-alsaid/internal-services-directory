using FluentValidation;
using MultCo_ISD_API.V1.DTO;

namespace MultCo_ISD_API.V1.Validators
{
    public class LanguageV1DTOValidator : AbstractValidator<LanguageV1DTO>
    {
        public LanguageV1DTOValidator()
        {
            RuleFor(x => x.LanguageID).NotNull();

            RuleFor(x => x.LanguageName).NotNull().MaximumLength(10);

        }
    }
}
