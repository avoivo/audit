using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Probanx.TransactionAudit.Core.Models;
using Probanx.TransactionAudit.Core.Services;
using RabbitMQ.Client;

namespace Probanx.TransactionAudit.Web.Services
{
    internal class RabbitMqMessageDispatcher : IMessageDispatcher
    {
        private readonly IConnection _connection;

        public RabbitMqMessageDispatcher(IConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.CreateConnection();
        }

        ~RabbitMqMessageDispatcher()
        {
            _connection.Dispose();
        }


        public Task Dispatch(Message message)
        {
            using (var channel = _connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);



                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                channel.BasicPublish(exchange: "",
                                    routingKey: "hello",
                                    basicProperties: null,
                                    body: body);
            }

            return Task.CompletedTask;
        }
    }
}