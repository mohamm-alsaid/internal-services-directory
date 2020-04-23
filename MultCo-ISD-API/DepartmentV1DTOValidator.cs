using FluentValidation;
using MultCo_ISD_API.V1.DTO;

namespace MultCo_ISD_API.V1.Validators
{
    public class DepartmentV1DTOValidator : AbstractValidator<DepartmentV1DTO>
    {
        public DepartmentV1DTOValidator()
        {
            RuleFor(x => x.DepartmentID).NotNull();
            RuleFor(x => x.DepartmentCode).NotNull();
            RuleFor(x => x.DepartmentName).NotNull().MaximumLength(20);

        }
    }
}
