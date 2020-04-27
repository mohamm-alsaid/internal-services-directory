using FluentValidation;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.Validators
{
    public class DepartmentValidator : AbstractValidator<Department>
    {
        public DepartmentValidator()
        {
            RuleFor(x => x.DepartmentId).NotNull();
            RuleFor(x => x.DepartmentCode).NotNull();
            RuleFor(x => x.DepartmentName).NotNull().MaximumLength(20).WithMessage("long Department name"); ;
        }
    }
}
