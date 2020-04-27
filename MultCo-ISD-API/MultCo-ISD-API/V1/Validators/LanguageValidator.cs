using FluentValidation;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.V1.Validators
{
    public class LanguageValidator : AbstractValidator<Language>
    {
        public LanguageValidator()
        {
            RuleFor(x => x.LanguageId).NotNull();

            RuleFor(x => x.LanguageName).NotNull().MaximumLength(10).WithMessage("long Language name"); ;

        }
    }
}
