namespace Hotel.Common.Features.Financial.FinancialTransactionFeatures.Commands.Create
{
    public class CreateFinancialTransactionResult
    {
        public CreateFinancialTransactionResult(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}