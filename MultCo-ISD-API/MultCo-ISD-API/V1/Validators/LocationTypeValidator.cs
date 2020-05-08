using System.Data;
using FluentValidation;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.V1.Validators
{
    public class LocationTypeValidator : AbstractValidator<LocationType>
    {
        public LocationTypeValidator()
        {

            RuleFor(x => x.LocationTypeId)
                .NotNull().WithMessage("Location type id cannot be null");
            RuleFor(x => x.LocationTypeName)
                .NotNull().WithMessage("Location type name cannot be null")
                .MaximumLength(20).WithMessage("Location type name cannot be longer than 20");
        }
    }
}
