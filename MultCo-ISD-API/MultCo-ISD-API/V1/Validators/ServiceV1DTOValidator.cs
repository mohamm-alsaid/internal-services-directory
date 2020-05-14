using System.Data;
using FluentValidation;
using FluentValidation.Validators;
using MultCo_ISD_API.V1.DTO;

namespace MultCo_ISD_API.V1.Validators
{
    public class ServiceV1DTOValidator : AbstractValidator<ServiceV1DTO>
    {
        public ServiceV1DTOValidator()
        {
            RuleFor(x => x.ServiceId)
                .Empty().WithMessage("cannot specify service ID explicitly");

            RuleFor(x => x.ProgramId)
                .NotNull().WithMessage("program id cannot be null")
                .GreaterThan(0).WithMessage("program Id should start from 1");

            RuleFor(x => x.DivisionId)
                .NotNull().WithMessage("division id cannot be null")
                .GreaterThan(0).WithMessage("division Id should start from 1");
            
            RuleFor(x => x.ServiceName)
                .NotNull().WithMessage("service name cannot be null")
                .MaximumLength(20).WithMessage("service name cannot exceed 20");
          
            RuleFor(x => x.ServiceDescription)
                .NotNull().WithMessage("service Description cannot be null")
                .MaximumLength(50).WithMessage("service Description cannot exceed 50 chars");

            RuleFor(x => x.ExecutiveSummary)
                .NotNull().WithMessage("Service Executive Summary cannot be null")
                .MaximumLength(50).WithMessage("Service Executive Summary cannot exceed 50 chars");

            RuleFor(x => x.ServiceArea)
                .NotNull().WithMessage("Service Area cannot be null")
                .MaximumLength(20).WithMessage("SService Area cannot exceed 20 chars");

            RuleFor(x => x.ContactId)
                .GreaterThan(0).WithMessage("Contact Id should start from 1");

            RuleFor(x => x.EmployeeConnectMethod)
                .NotNull().WithMessage("Employee Connect Method cannot be null")
                .MaximumLength(50).WithMessage("Employee Connect Method cannot exceed 50 chars");

            RuleFor(x => x.CustomerConnectMethod)
                .NotNull().WithMessage("Customer Connect Method cannot be null")
                .MaximumLength(50).WithMessage("Customer Connect Method cannot exceed 50 chars");

            RuleFor(x => x.ExpirationDate)
                .NotNull().WithMessage("Expiration Date cannot be null");

            RuleFor(x => x.Active)
                .NotNull().WithMessage("Service active cannot be null");

            // validate Contact ID:
            RuleFor(x => x).Must(x => x.ContactId == null || x.ContactDTO == null)
                .WithMessage("either ContactDTO or ContactId can be specified (not both)");

            // apply all the validation rules for the following fields:
            RuleFor(x => x.ContactDTO).SetValidator(new ContactV1DTOValidator());

            RuleFor(x => x.DepartmentDTO).SetValidator(new DepartmentV1DTOValidator());

            RuleFor(x => x.DivisionDTO).SetValidator(new DivisionV1DTOValidator());
            
            RuleFor(x => x.ProgramDTO).SetValidator(new ProgramV1DTOValidator());
        }
    }
}
