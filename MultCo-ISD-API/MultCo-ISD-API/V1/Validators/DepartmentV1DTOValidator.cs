using FluentValidation;
using MultCo_ISD_API.Models;

using MultCo_ISD_API.V1.DTO;
namespace MultCo_ISD_API.V1.Validators
{
    public class DepartmentV1DTOValidator : AbstractValidator<DepartmentV1DTO>
    {
        public DepartmentV1DTOValidator()
        {
            RuleFor(x => x.DepartmentId)
               .Empty().WithMessage("cannot specify department ID explicitly");

            RuleFor(x => x.DepartmentCode)
                .NotNull().WithMessage("Department code cannot be null");

            RuleFor(x => x.DepartmentName)
                .NotNull().WithMessage("Department name cannot be null")
                .MaximumLength(30).WithMessage("Department name cannot exceed 30 characters"); ;
        }
    }
}
