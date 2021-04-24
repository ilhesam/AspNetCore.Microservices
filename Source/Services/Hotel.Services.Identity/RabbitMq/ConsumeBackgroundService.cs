using System.Threading;
using System.Threading.Tasks;
using Hotel.Common.Features.Identity.Commands.SignIn;
using Hotel.Common.Features.Identity.Commands.SignUp;
using Hotel.Common.Features.Identity.Commands.ValidateToken;
using Hotel.Common.Models;
using Hotel.Common.RabbitMq;
using Hotel.Common.RabbitMq.Manager;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hotel.Services.Identity.RabbitMq
{
    public class ConsumeIdentityRabbitMqBackgroundService : ConsumeRabbitMqBackgroundService
    {
        public ConsumeIdentityRabbitMqBackgroundService(IRabbitMqManager rabbitMq, IMediator mediator, ILogger<ConsumeRabbitMqBackgroundService> logger) : base(rabbitMq, mediator, logger)
        {
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            RabbitMq.Consume<SignUpCommand>(Received<SignUpCommand, Result<SignUpResult>>);
            RabbitMq.Consume<SignInCommand>(Received<SignInCommand, Result<SignInResult>>);
            RabbitMq.Consume<ValidateTokenCommand>(Received<ValidateTokenCommand, Result<ValidateTokenResult>>);

            return base.ExecuteAsync(stoppingToken);
        }
    }
}