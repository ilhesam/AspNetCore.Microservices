using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Hotel.Common.RabbitMq.Manager
{
    public class RabbitMqManager : IRabbitMqManager
    {
        protected readonly DefaultObjectPool<IModel> ObjectPool;

        public RabbitMqManager(IPooledObjectPolicy<IModel> objectPolicy)
        {
            ObjectPool = new DefaultObjectPool<IModel>(objectPolicy, Environment.ProcessorCount * 2); ;
        }

        public IBasicProperties CreateBasicProperties()
        {
            var channel = ObjectPool.Get();

            try
            {
                return channel.CreateBasicProperties();
            }
            catch
            {
                throw;
            }
            finally
            {
                ObjectPool.Return(channel);
            }
        }

        public void Ack(ulong deliveryTag, bool multiple)
        {
            var channel = ObjectPool.Get();

            try
            {
                channel.BasicAck(deliveryTag: deliveryTag, multiple: multiple);
            }
            catch
            {
                throw;
            }
            finally
            {
                ObjectPool.Return(channel);
            }
        }

        public void Publish<TMessage>(TMessage message, string queueName) where TMessage : class
        {
            if (message == null) return;

            var channel = ObjectPool.Get();

            try
            {
                channel.QueueDeclare(queueName, false, false, false, null);

                var sendBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish("", queueName, properties, sendBytes);
            }
            catch
            {
                throw;
            }
            finally
            {
                ObjectPool.Return(channel);
            }
        }

        public void Publish<TMessage>(TMessage message, string queueName, IBasicProperties basicProperties) where TMessage : class
        {
            if (message == null) return;

            var channel = ObjectPool.Get();

            try
            {
                channel.QueueDeclare(queueName, false, false, false, null);

                var sendBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                channel.BasicPublish("", queueName, basicProperties, sendBytes);
            }
            catch
            {
                throw;
            }
            finally
            {
                ObjectPool.Return(channel);
            }
        }

        public void Publish<TMessage>(TMessage message) where TMessage : class
            => Publish<TMessage>(message, typeof(TMessage).GenericTypeArguments?.FirstOrDefault()?.Name ?? typeof(TMessage).Name // Get name of generic argument when TMessage is generic
                                                                                                                                 // For example: TMessage = Result<SubResult> | Queue name = SubResult
            );

        public void Publish<TMessage>(TMessage message, IBasicProperties basicProperties) where TMessage : class
            => Publish<TMessage>(message,
                typeof(TMessage).GenericTypeArguments?.FirstOrDefault()?.Name ?? typeof(TMessage).Name, // Get name of generic argument when TMessage is generic
                                                                                                        // For example: TMessage = Result<SubResult> | Queue name = SubResult
                basicProperties);

        public TResponse Publish<TMessage, TResponse>(TMessage message, string queueName, string replyQueueName) where TResponse : class where TMessage : class
        {
            if (message == null) return null;

            var channel = ObjectPool.Get();

            try
            {
                channel.QueueDeclare(replyQueueName, false, false, false, null);
                var consumer = new AsyncEventingBasicConsumer(channel);

                var properties = channel.CreateBasicProperties();
                var correlationId = Guid.NewGuid().ToString();
                properties.CorrelationId = correlationId;
                properties.ReplyTo = replyQueueName;
                properties.Persistent = true;

                var respQueue = new BlockingCollection<string>();

                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();

                    if (ea.BasicProperties.CorrelationId == correlationId)
                    {
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        respQueue.Add(Encoding.UTF8.GetString(body));
                    }

                    await Task.Yield();
                };

                var sendBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                channel.BasicPublish("", queueName, properties, sendBytes);
                var consumerTag = channel.BasicConsume(consumer, replyQueueName, autoAck: false);

                var response = respQueue.Take(); // Waits for the response to be returned and then takes the response
                channel.BasicCancel(consumerTag); // Cancels consumer when response took so that the event does not run again
                return JsonConvert.DeserializeObject<TResponse>(response);
            }
            catch
            {
                throw;
            }
            finally
            {
                ObjectPool.Return(channel);
            }
        }

        public TResponse Publish<TMessage, TResponse>(TMessage message) where TMessage : class where TResponse : class
            => Publish<TMessage, TResponse>(message,
                typeof(TMessage).GenericTypeArguments?.FirstOrDefault()?.Name ?? typeof(TMessage).Name, // Get name of generic argument when TMessage is generic
                                                                                                        // For example: TMessage = Result<SubResult> | Queue name = SubResult
                typeof(TResponse).GenericTypeArguments?.FirstOrDefault()?.Name ?? typeof(TResponse).Name // Get name of generic argument when TResponse is generic
                                                                                                         // For example: TResponse = Result<SubResult> | Queue name = SubResult
                );

        public void Consume(string queueName, EventHandler<BasicDeliverEventArgs> received)
        {
            var channel = ObjectPool.Get();

            try
            {
                channel.QueueDeclare(queueName, false, false, false, null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += received;

                channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
            }
            catch
            {
                throw;
            }
            finally
            {
                ObjectPool.Return(channel);
            }
        }

        public void Consume<TMessage>(EventHandler<BasicDeliverEventArgs> received) where TMessage : class
            => Consume(typeof(TMessage).GenericTypeArguments?.FirstOrDefault()?.Name ?? typeof(TMessage).Name, // Get name of generic argument when TMessage is generic
                                                                                                               // For example: TMessage = Result<SubResult> | Queue name = SubResult)
                received);

        public void Consume(string queueName, AsyncEventHandler<BasicDeliverEventArgs> received)
        {
            var channel = ObjectPool.Get();

            try
            {
                channel.QueueDeclare(queueName, false, false, false, null);

                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.Received += received;

                channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
            }
            catch
            {
                throw;
            }
            finally
            {
                ObjectPool.Return(channel);
            }
        }

        public void Consume<TMessage>(AsyncEventHandler<BasicDeliverEventArgs> received) where TMessage : class
            => Consume(typeof(TMessage).GenericTypeArguments?.FirstOrDefault()?.Name ?? typeof(TMessage).Name, // Get name of generic argument when TMessage is generic
                // For example: TMessage = Result<SubResult> | Queue name = SubResult)
                received);
    }
}