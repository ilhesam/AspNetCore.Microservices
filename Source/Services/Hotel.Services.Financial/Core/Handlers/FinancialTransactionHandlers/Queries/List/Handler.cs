using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hotel.Common.Features.Financial.FinancialTransactionFeatures.Queries.List;
using Hotel.Common.Models;
using Hotel.Services.Financial.Database.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hotel.Services.Financial.Core.Handlers.FinancialTransactionHandlers.Queries.List
{
    public class ListFinancialTransactionsQueryHandler : IRequestHandler<ListFinancialTransactionsQuery, Result<ListFinancialTransactionsResult>>
    {
        private readonly IFinancialTransactionRepository _repository;
        private readonly ILogger<ListFinancialTransactionsQueryHandler> _logger;

        public ListFinancialTransactionsQueryHandler(IFinancialTransactionRepository repository, ILogger<ListFinancialTransactionsQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Result<ListFinancialTransactionsResult>> Handle(ListFinancialTransactionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entities = await _repository.ListAsync();

                return Result<ListFinancialTransactionsResult>.Success(new ListFinancialTransactionsResult(entities.Cast<object>().ToList()));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                return Result<ListFinancialTransactionsResult>.Fail(Error.Default());
            }
        }
    }
}