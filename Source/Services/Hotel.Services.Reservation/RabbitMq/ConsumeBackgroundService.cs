using System.Threading;
using System.Threading.Tasks;
using Hotel.Common.Features.Reservation.RoomReserveFeatures.Commands.Create;
using Hotel.Common.Features.Reservation.RoomReserveFeatures.Queries.List;
using Hotel.Common.Models;
using Hotel.Common.RabbitMq;
using Hotel.Common.RabbitMq.Manager;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hotel.Services.Reservation.RabbitMq
{
    public class ConsumeReservationRabbitMqBackgroundService : ConsumeRabbitMqBackgroundService
    {
        public ConsumeReservationRabbitMqBackgroundService(IRabbitMqManager rabbitMq, IMediator mediator, ILogger<ConsumeRabbitMqBackgroundService> logger) : base(rabbitMq, mediator, logger)
        {
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            RabbitMq.Consume<ListRoomReservesQuery>(Received<ListRoomReservesQuery, Result<ListRoomReservesResult>>);
            RabbitMq.Consume<CreateRoomReserveCommand>(Received<CreateRoomReserveCommand, Result<CreateRoomReserveResult>>);

            return base.ExecuteAsync(stoppingToken);
        }
    }
}