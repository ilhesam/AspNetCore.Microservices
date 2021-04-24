using System.Collections.Generic;

namespace Hotel.Common.Features.Financial.FinancialTransactionFeatures.Queries.List
{
    public class ListFinancialTransactionsResult
    {
        public ListFinancialTransactionsResult(List<object> financialTransactions)
        {
            FinancialTransactions = financialTransactions;
        }

        public List<object> FinancialTransactions { get; set; }
    }
}