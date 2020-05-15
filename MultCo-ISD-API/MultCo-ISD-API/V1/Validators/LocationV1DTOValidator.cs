using System.Data;
using FluentValidation;
using MultCo_ISD_API.Models;
using MultCo_ISD_API.V1.DTO;


namespace MultCo_ISD_API.V1.Validators
{
    public class LocationV1DTOValidator : AbstractValidator<LocationV1DTO>
    {
        public LocationV1DTOValidator()
        {
            RuleFor(x => x.LocationId)
                .Empty().WithMessage("cannot specify location ID explicitly");

            RuleFor(x => x.LocationTypeId);
                //.NotNull().WithMessage("Location Type ID cannot be null");

            RuleFor(x => x.LocationName)
                //.NotNull().WithMessage("Location name cannot be null")
                .MaximumLength(30).WithMessage("Location name cannot exceed 30 characters");

            //RuleFor(x => x.BuildingId)
                //.NotNull().WithMessage("Building ID cannot be null");

            RuleFor(x => x.LocationAddress)
                //.NotNull().WithMessage("Location Address cannot be null")
                .MaximumLength(50).WithMessage("Location Address cannot exceed 50 characters");

            RuleFor(x => x.RoomNumber)
                //.NotNull().WithMessage("Room Number cannot be null")
                .MaximumLength(4).WithMessage("Room number cannot exceed 4");

            RuleFor(x => x.FloorNumber)
                //.NotNull().WithMessage("Room Number cannot be null")
                .MaximumLength(3).WithMessage("floor number cannot exceed 3");

        }
    }
}
