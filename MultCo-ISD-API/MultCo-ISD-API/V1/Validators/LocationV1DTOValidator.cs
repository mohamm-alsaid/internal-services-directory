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

            RuleFor(x => x.LocationTypeId)
                .NotNull().WithMessage("Location Type ID cannot be null");

            RuleFor(x => x.LocationName)
                .MaximumLength(255).WithMessage("Location name cannot exceed 255 characters");

            RuleFor(x => x.BuildingId)
                .MaximumLength(255).WithMessage("Location name cannot exceed 255 characters");

            RuleFor(x => x.LocationAddress)
                .MaximumLength(255).WithMessage("Location Address cannot exceed 255 characters");

            RuleFor(x => x.RoomNumber)
                .MaximumLength(255).WithMessage("Room number cannot exceed 255");

            RuleFor(x => x.FloorNumber)
                .MaximumLength(255).WithMessage("floor number cannot exceed 255");

        }
    }
}
