using FluentValidation;
using MultCo_ISD_API.V1.DTO;

namespace MultCo_ISD_API.V1.Validators
{
    public class ContactV1DTOValidator : AbstractValidator<ContactV1DTO>
    {
        public ContactV1DTOValidator()
        {
            RuleFor(x => x.ContactID).NotNull();
            RuleFor(x => x.ContactName).NotNull().MaximumLength(50);
            RuleFor(x => x.PhoneNumber).NotNull().MaximumLength(11);
            RuleFor(x => x.EmailAddress).NotNull().MaximumLength(20);
        }
    }
}
