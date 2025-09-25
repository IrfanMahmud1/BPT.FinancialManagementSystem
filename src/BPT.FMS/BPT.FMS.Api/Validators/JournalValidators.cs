using BPT.FMS.Domain.Dtos;
using FluentValidation;

namespace BPT.FMS.Api.Validators
{
    public class JournalEntryDtoValidator : AbstractValidator<JournalEntryDto>
    {
        public JournalEntryDtoValidator()
        {
            RuleFor(x => x.ChartOfAccountId).NotEmpty();
            RuleFor(x => x.Debit).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Credit).GreaterThanOrEqualTo(0);
        }
    }

    public class JournalDtoValidator : AbstractValidator<JournalDto>
    {
        public JournalDtoValidator()
        {
            RuleFor(x => x.Type).NotEmpty();
            RuleFor(x => x.ReferenceNo).NotEmpty();
            RuleFor(x => x.Date).NotEmpty();
            RuleFor(x => x.Entries)
                .NotNull()
                .Must(list => list.Count > 0).WithMessage("At least one entry is required")
                .ForEach(rule => rule.SetValidator(new JournalEntryDtoValidator()));

            RuleFor(x => x)
                .Must(x => x.Entries.Sum(e => e.Debit) == x.Entries.Sum(e => e.Credit))
                .WithMessage("Total debits must equal total credits");
        }
    }
}

