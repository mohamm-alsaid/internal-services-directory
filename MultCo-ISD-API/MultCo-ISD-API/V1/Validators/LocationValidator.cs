using System.Data;
using FluentValidation;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.V1.Validators
{
    public class LocationValidator : AbstractValidator<Location>
    {
        public LocationValidator()
        {
            RuleFor(x => x.LocationId).NotNull()
                .WithMessage("Location ID cannot be null");

            RuleFor(x => x.LocationTypeId).NotNull()
                .WithMessage("Location Type ID cannot be null");

            RuleFor(x => x.LocationName)
                .NotNull().WithMessage("Location name cannot be null")
                .MaximumLength(20).WithMessage("Location name cannot be longer than 20");

            RuleFor(x => x.BuildingId).NotNull()
                .WithMessage("Location ID cannot be null");

            RuleFor(x => x.LocationAddress)
                .NotNull().WithMessage("Location Address cannot be null")
                .MaximumLength(50).WithMessage("Location Address cannot be longer than 50");

            RuleFor(x => x.RoomNumber)
                .NotNull().WithMessage("Room Number cannot be null");

            RuleFor(x => x.FloorNumber)
                .NotNull().WithMessage("Room Number cannot be null");
        }
    }
}
