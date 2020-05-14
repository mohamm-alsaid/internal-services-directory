using System.Data;
using FluentValidation;
using MultCo_ISD_API.Models;
using MultCo_ISD_API.V1.DTO;

namespace MultCo_ISD_API.V1.Validators
{
    public class LocationTypeV1DTOValidator : AbstractValidator<LocationTypeV1DTO>
    {
        public LocationTypeV1DTOValidator()
        {
            RuleFor(x => x.LocationTypeId)
                .Empty().WithMessage("cannot specify location type ID explicitly");

            RuleFor(x => x.LocationTypeName)
                .NotNull().WithMessage("Location type name cannot be null")
                .MaximumLength(30).WithMessage("Location type name cannot exceed 30 characters");
        }
    }
}
