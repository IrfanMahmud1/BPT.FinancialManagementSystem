using BPT.FMS.Domain.Dtos;
using FluentValidation;

namespace BPT.FMS.Api.Validators
{
    public class ChartOfAccountValidator : AbstractValidator<ChartOfAccountDto>
    {
        public ChartOfAccountValidator()
        {
            RuleFor(x => x.AccountName)
                .NotEmpty().WithMessage("Account name is required")
                .MaximumLength(100).WithMessage("Account name cannot exceed 100 characters");

            RuleFor(x => x.AccountType)
                .NotEmpty().WithMessage("Account type is required");

            RuleFor(x => x.ParentId)
                .NotEqual(x => x.Id).WithMessage("Parent account cannot be itself");

            //RuleFor(x => x.Id)
            //    .NotEqual(Guid.Empty).When(x => x.Id != Guid.Empty);
        }
    }
}
