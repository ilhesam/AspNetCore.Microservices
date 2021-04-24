using FluentValidation;
using Hotel.ApiGateways.Main.Areas.Financial.Enums;

namespace Hotel.ApiGateways.Main.Areas.Financial.ViewModels.FinancialTransaction.Create
{
    public class CreateFinancialTransactionInput
    {
        public string About { get; set; }
        public double Amount { get; set; }
        public FinancialTransactionTypesEnum Type { get; set; }
    }

    public class CreateFinancialTransactionInputValidator : AbstractValidator<CreateFinancialTransactionInput>
    {
        public CreateFinancialTransactionInputValidator()
        {
            RuleFor(e => e.About)
                .NotNull()
                .NotEmpty()
                .MaximumLength(256);

            RuleFor(e => e.Amount)
                .GreaterThan(0);

            RuleFor(e => e.Type)
                .IsInEnum();
        }
    }
}