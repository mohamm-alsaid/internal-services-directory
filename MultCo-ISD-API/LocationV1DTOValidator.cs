using FluentValidation;
using MultCo_ISD_API.V1.DTO;

namespace MultCo_ISD_API.V1.Validators
{
    public class LocationV1DTOValidator : AbstractValidator<LocationV1DTO>
    {
        public LocationV1DTOValidator()
        {
            RuleFor(x => x.LocationID).NotNull();
            RuleFor(x => x.LocationTypeID).NotNull();
            RuleFor(x => x.LocationName).NotNull().MaximumLength(20);

        }
    }
}
