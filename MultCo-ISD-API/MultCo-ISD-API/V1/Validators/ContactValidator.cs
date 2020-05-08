using FluentValidation;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.V1.Validators
{
    public class ContactValidator : AbstractValidator<Contact>
    {
        public ContactValidator()
        {
            RuleFor(x => x.ContactId).NotNull();
            RuleFor(x => x.ContactName).NotNull().MaximumLength(20);
            RuleFor(x => x.PhoneNumber).NotNull().MaximumLength(13).Matches("^\\d{3}-\\d{3}-\\d{4}$");
            RuleFor(x => x.EmailAddress).NotNull().MaximumLength(20).EmailAddress();
        }
    }
}
