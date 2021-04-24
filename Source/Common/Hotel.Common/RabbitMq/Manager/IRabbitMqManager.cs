using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Hotel.Common.RabbitMq.Manager
{
    public interface IRabbitMqManager
    {
        IBasicProperties CreateBasicProperties();
        void Ack(ulong deliveryTag, bool multiple);
        void Publish<TMessage>(TMessage message, string queueName) where TMessage : class;
        void Publish<TMessage>(TMessage message, string queueName, IBasicProperties basicProperties) where TMessage : class;
        void Publish<TMessage>(TMessage message) where TMessage : class;
        void Publish<TMessage>(TMessage message, IBasicProperties basicProperties) where TMessage : class;
        TResponse Publish<TMessage, TResponse>(TMessage message, string queueName, string replyQueueName) where TResponse : class where TMessage : class;
        TResponse Publish<TMessage, TResponse>(TMessage message) where TResponse : class where TMessage : class;
        void Consume(string queueName, EventHandler<BasicDeliverEventArgs> received);
        void Consume<TMessage>(EventHandler<BasicDeliverEventArgs> received) where TMessage : class;
        void Consume(string queueName, AsyncEventHandler<BasicDeliverEventArgs> received);
        void Consume<TMessage>(AsyncEventHandler<BasicDeliverEventArgs> received) where TMessage : class;
    }
}