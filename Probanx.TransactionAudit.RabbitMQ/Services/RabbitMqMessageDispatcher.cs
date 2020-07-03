using System.Text;
using System.Threading.Tasks;
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

                string message1 = "Hello World!";
                var body = Encoding.UTF8.GetBytes(message1);

                channel.BasicPublish(exchange: "",
                                    routingKey: "hello",
                                    basicProperties: null,
                                    body: body);
            }

            return Task.CompletedTask;
        }
    }
}