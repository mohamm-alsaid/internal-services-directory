using System.Data;
using FluentValidation;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.V1.Validators
{
    public class LocationValidator : AbstractValidator<Location>
    {
        public LocationValidator()
        {
            RuleFor(x => x.LocationId).NotNull();
            RuleFor(x => x.LocationTypeId).NotNull();
            RuleFor(x => x.LocationName).NotNull().MaximumLength(20);
            RuleFor(x => x.BuildingId).NotNull();
            RuleFor(x => x.LocationAddress).NotNull().MaximumLength(50);
            RuleFor(x => x.RoomNumber).NotNull();
            RuleFor(x => x.FloorNumber).NotNull();
        }
    }
}
