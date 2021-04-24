using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hotel.Common.RabbitMq.Manager;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;

namespace Hotel.Common.RabbitMq
{
    public class ConsumeRabbitMqBackgroundService : BackgroundService
    {
        protected readonly IRabbitMqManager RabbitMq;
        protected readonly IMediator Mediator;
        protected readonly ILogger<ConsumeRabbitMqBackgroundService> Logger;

        public ConsumeRabbitMqBackgroundService(IRabbitMqManager rabbitMq, IMediator mediator, ILogger<ConsumeRabbitMqBackgroundService> logger)
        {
            RabbitMq = rabbitMq;
            Mediator = mediator;
            Logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            return Task.CompletedTask;
        }

        public async Task Received<TCommand, TResult>(object model, BasicDeliverEventArgs ea) where TCommand : class where TResult : class
        {
            TResult result = default;

            var body = ea.Body.ToArray();
            var props = ea.BasicProperties;
            var replyProps = RabbitMq.CreateBasicProperties();
            replyProps.CorrelationId = props.CorrelationId;
            replyProps.Persistent = true;

            try
            {
                var message = Encoding.UTF8.GetString(body);
                var command = JsonConvert.DeserializeObject<TCommand>(message);
                result = (TResult) await Mediator.Send(command!);
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
            }
            finally
            {
                RabbitMq.Publish(result, replyProps);
                RabbitMq.Ack(deliveryTag: ea.DeliveryTag, multiple: false);
            }
        }
    }
}