using FluentValidation;
using MultCo_ISD_API.Models;

using MultCo_ISD_API.V1.DTO;

namespace MultCo_ISD_API.V1.Validators
{
    public class LanguageV1DTOValidator : AbstractValidator<LanguageV1DTO>
    {
        public LanguageV1DTOValidator()
        {
            RuleFor(x => x.LanguageId)
                .Empty().WithMessage("cannot specify language ID explicitly");

            RuleFor(x => x.LanguageName)
                //.NotNull().WithMessage("Language name cannot be null")
                .MaximumLength(30).WithMessage("Language name cannot exceed 30 characters"); ;
        }
    }
}
