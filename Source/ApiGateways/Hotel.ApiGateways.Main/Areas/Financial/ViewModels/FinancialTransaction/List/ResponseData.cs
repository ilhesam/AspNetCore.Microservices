using System.Collections.Generic;
using Hotel.ApiGateways.Main.Areas.Financial.ViewModels.FinancialTransaction.Get;

namespace Hotel.ApiGateways.Main.Areas.Financial.ViewModels.FinancialTransaction.List
{
    public class FinancialTransactionsResponseData
    {
        public FinancialTransactionsResponseData(List<FinancialTransactionDto> financialTransactions)
        {
            FinancialTransactions = financialTransactions;
        }

        public List<FinancialTransactionDto> FinancialTransactions { get; set; }
    }
}