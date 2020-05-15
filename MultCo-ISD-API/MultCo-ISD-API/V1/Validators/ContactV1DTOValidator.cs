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
                .MaximumLength(255).WithMessage("Contact name cannot be longer than 255");

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(255).WithMessage("Contact phone number cannot exceed 255 numbers")
                .Matches("^\\d{3}-\\d{3}-\\d{4}$").WithMessage("contact phone number must follow xxx-xxx-xxxx format");

            RuleFor(x => x.EmailAddress)
                .MaximumLength(255).WithMessage("contact email address cannot exceed 255 characters")
                .EmailAddress().WithMessage("contact email address is not in the correct format");
        }
    }
}
