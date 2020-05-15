using FluentValidation;
using MultCo_ISD_API.V1.DTO;

namespace MultCo_ISD_API.V1.Validators
{
    public class ContactV1DTOValidator : AbstractValidator<ContactV1DTO>
    {
        public ContactV1DTOValidator()
        {
            RuleFor(x => x.ContactId)
               .Empty().WithMessage("cannot specify contact ID explicitly");

            RuleFor(x => x.ContactName)
               // .NotNull().WithMessage("Contact name cannot be null")
                .MaximumLength(20).WithMessage("Contact name cannot be longer than 20");

            RuleFor(x => x.PhoneNumber)
               // .NotNull().WithMessage("Contact name cannot be null")
                .MaximumLength(13).WithMessage("Contact phone number cannot exceed 13 numbers")
                .Matches("^\\d{3}-\\d{3}-\\d{4}$").WithMessage("contact phone number must follow xxx-xxx-xxxx format");

            RuleFor(x => x.EmailAddress)
                .NotNull().WithMessage("contact email address cannot be null")
                .MaximumLength(30).WithMessage("contact email address cannot exceed 30 characters")
                .EmailAddress().WithMessage("contact email address is not in the correct format");
        }
    }
}
