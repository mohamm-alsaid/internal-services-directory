using System.Data;
using FluentValidation;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.Validators
{
    public class LocationTypeValidator : AbstractValidator<LocationType>
    {
        public LocationTypeValidator()
        {

            RuleFor(x => x.LocationTypeId).NotNull();
            RuleFor(x => x.LocationTypeName).NotNull().MaximumLength(20).WithMessage("long Location Type name"); ;
        }
    }
}
