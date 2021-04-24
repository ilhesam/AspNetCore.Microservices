namespace Hotel.ApiGateways.Main.Areas.Financial.ViewModels.FinancialTransaction.Get
{
    public class FinancialTransactionDto
    {
        public string About { get; set; }
        public double Amount { get; set; }
        public byte Type { get; set; }
    }
}