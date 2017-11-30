using System;
using RabbitMQ.Client;

namespace Sma.Stm.EventBusRabbitMq
{
    public interface IRabbitMqPersistentConnection
        : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}
