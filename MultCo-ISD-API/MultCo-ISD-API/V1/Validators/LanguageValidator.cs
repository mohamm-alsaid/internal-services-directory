using FluentValidation;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.V1.Validators
{
    public class LanguageValidator : AbstractValidator<Language>
    {
        public LanguageValidator()
        {
            RuleFor(x => x.LanguageId)
                .NotNull().WithMessage("Language id cannot be null");

            RuleFor(x => x.LanguageName)
                .NotNull().WithMessage("Language name cannot be null")
                .MaximumLength(20).WithMessage("Language name cannot be longer than 20"); ;

        }
    }
}
