using System.Data;
using FluentValidation;
using FluentValidation.Validators;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.V1.Validators
{
    public class ServiceValidator : AbstractValidator<Service>
    {
        public ServiceValidator()
        {
            RuleFor(x => x.ServiceId).NotNull();
            //RuleFor(x => x.ProgramId).NotEmpty();
            //RuleFor(x => x.DepartmentId.NotEmpty();
            RuleFor(x => x.DivisionId).NotNull();
            RuleFor(x => x.ServiceName).NotNull().MaximumLength(20);
            RuleFor(x => x.ServiceDescription).NotNull().MaximumLength(50);
            RuleFor(x => x.ExecutiveSummary).NotNull().MaximumLength(50).WithMessage("you f'ed up");
            RuleFor(x => x.ServiceArea).NotNull().MaximumLength(20);
            //RuleFor(x => x.ContactId).NotNull();
            RuleFor(x => x.EmployeeConnectMethod).NotNull().MaximumLength(50);
            RuleFor(x => x.CustomerConnectMethod).NotNull().MaximumLength(50);
            //RuleFor(x => x.ExpirationDate).NotNull();

        }
    }
}
