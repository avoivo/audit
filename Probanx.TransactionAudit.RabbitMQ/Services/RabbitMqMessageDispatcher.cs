using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Probanx.TransactionAudit.Core.Services;
using RabbitMQ.Client;

namespace Probanx.TransactionAudit.Web.Services
{
    internal class RabbitMqMessageDispatcher<T> : IMessageDispatcher<T>
    {
        private readonly IConnection _connection;
        private readonly string _queueName;

        public RabbitMqMessageDispatcher(IConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.CreateConnection();
            _queueName = typeof(T).Name;
        }

        ~RabbitMqMessageDispatcher()
        {
            _connection.Dispose();
        }


        public Task Dispatch(T message)
        {
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

            return Task.CompletedTask;
        }
    }
}