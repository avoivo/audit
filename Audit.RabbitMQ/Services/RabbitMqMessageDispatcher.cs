using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Audit.Core.Services;
using RabbitMQ.Client;
using Microsoft.Extensions.Logging;

namespace Audit.Web.Services
{
    internal class RabbitMqMessageDispatcher<T> : IMessageDispatcher<T>
    {
        private readonly ILogger _logger;
        private readonly IConnection _connection;
        private readonly string _queueName;

        public RabbitMqMessageDispatcher(ILogger<RabbitMqMessageDispatcher<T>> logger, IConnectionFactory connectionFactory)
        {
            _logger = logger;
            _connection = connectionFactory.CreateConnection();
            _queueName = typeof(T).Name;
        }

        ~RabbitMqMessageDispatcher()
        {
            _connection.Dispose();
        }


        public Task Dispatch(T message)
        {
            _logger.LogTrace("Dispatch() enter");
            _logger.LogDebug("Dispatch({MESSAGE})", message);

            using (var channel = _connection.CreateModel())
            {
                channel.QueueDeclare(queue: _queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                channel.BasicPublish(exchange: "",
                                    routingKey: _queueName,
                                    basicProperties: null,
                                    body: body);
            }

            _logger.LogTrace("Dispatch() exit");
            return Task.CompletedTask;
        }
    }
}