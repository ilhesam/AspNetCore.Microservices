using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Hotel.Common.RabbitMq
{
    public class RabbitMqModelPooledObjectPolicy : IPooledObjectPolicy<IModel>
    {
        protected readonly RabbitMqOptions Options;
        protected readonly IConnection Connection;

        public RabbitMqModelPooledObjectPolicy(IOptions<RabbitMqOptions> options)
        {
            Options = options.Value;
            Connection = GetConnection();
        }

        private IConnection GetConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = Options.HostName,
                UserName = Options.UserName,
                Password = Options.Password,
                Port = Options.Port,
                VirtualHost = Options.VHost,
                DispatchConsumersAsync = true
            };

            return factory.CreateConnection();
        }

        public IModel Create() => Connection.CreateModel();

        public bool Return(IModel obj)
        {
            if (obj.IsOpen)
            {
                return true;
            }
            else
            {
                obj?.Dispose();
                return false;
            }
        }
    }
}