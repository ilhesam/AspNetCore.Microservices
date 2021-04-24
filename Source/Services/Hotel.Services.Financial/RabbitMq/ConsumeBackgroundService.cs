using System.Threading;
using System.Threading.Tasks;
using Hotel.Common.Features.Financial.FinancialTransactionFeatures.Commands.Create;
using Hotel.Common.Features.Financial.FinancialTransactionFeatures.Queries.List;
using Hotel.Common.Models;
using Hotel.Common.RabbitMq;
using Hotel.Common.RabbitMq.Manager;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hotel.Services.Financial.RabbitMq
{
    public class ConsumeFinancialRabbitMqBackgroundService : ConsumeRabbitMqBackgroundService
    {
        public ConsumeFinancialRabbitMqBackgroundService(IRabbitMqManager rabbitMq, IMediator mediator, ILogger<ConsumeRabbitMqBackgroundService> logger) : base(rabbitMq, mediator, logger)
        {
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            RabbitMq.Consume<CreateFinancialTransactionCommand>(Received<CreateFinancialTransactionCommand, Result<CreateFinancialTransactionResult>>);
            RabbitMq.Consume<ListFinancialTransactionsQuery>(Received<ListFinancialTransactionsQuery, Result<ListFinancialTransactionsResult>>);

            return base.ExecuteAsync(stoppingToken);
        }
    }
}