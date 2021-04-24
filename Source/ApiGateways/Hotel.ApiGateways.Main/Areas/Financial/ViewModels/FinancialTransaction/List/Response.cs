using Hotel.ApiGateways.Main.Common.ViewModels;

namespace Hotel.ApiGateways.Main.Areas.Financial.ViewModels.FinancialTransaction.List
{
    public class FinancialTransactionsResponse : ApiResponse<FinancialTransactionsResponseData>
    {
        public FinancialTransactionsResponse(string code, string message, FinancialTransactionsResponseData data) : base(code, message, data)
        {
        }
    }
}