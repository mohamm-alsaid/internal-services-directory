using System.Data;
using FluentValidation;
using FluentValidation.Validators;
using MultCo_ISD_API.V1.DTO;

namespace MultCo_ISD_API.V1.Validators
{
    public class LocationTypeValidator : AbstractValidator<LocationTypeV1DTO>
    {
        public LocationTypeValidator()
        {

            //RuleFor(x => x.LocationTypeID).NotNull();
            //RuleFor(x => x.LocationTypeName).NotNull().MaximumLength(20);
        }
    }
}
