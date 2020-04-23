using System.Data;
using FluentValidation;
using FluentValidation.Validators;
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
            RuleFor(x => x.BuildingID).NotNull();
            RuleFor(x => x.LocationAddress).NotNull().MaximumLength(50);
            RuleFor(x => x.RoomNumber).NotNull();
            RuleFor(x => x.FloorNumber).NotNull();
        }
    }
}
