using System.Text;
using System.Text.Json;
using Probanx.TransactionAudit.Core.Services;
using RabbitMQ.Client;
using System;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Probanx.TransactionAudit.Web.Services
{
    internal class RabbitMqMessageConsumer<T> : IMessageConsumer<T>
    {
        private readonly ILogger _logger;
        private readonly IConnection _connection;
        private readonly string _queueName;

        public RabbitMqMessageConsumer(ILogger<RabbitMqMessageConsumer<T>> logger, IConnectionFactory connectionFactory)
        {
            _logger = logger;
            _connection = connectionFactory.CreateConnection();
            _queueName = typeof(T).Name;
        }

        ~RabbitMqMessageConsumer()
        {
            _connection.Dispose();
        }


        public Task Consume(Action<T> consumeAction)
        {
            _logger.LogTrace("Consume() enter");
            _logger.LogDebug("Consume({ACTION})", consumeAction);

            using (var channel = _connection.CreateModel())
            {
                channel.QueueDeclare(queue: _queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    _logger.LogDebug("[x] Received {PAYLOAD}", message);

                    try
                    {
                        var messageAsJson = JsonSerializer.Deserialize<T>(message);
                        consumeAction(messageAsJson);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Exception Thrown");
                    }

                };
                channel.BasicConsume(queue: _queueName,
                                    autoAck: true,
                                    consumer: consumer);

                Thread.Sleep(Timeout.Infinite);

            }

            _logger.LogTrace("Consume() exit");
            return Task.CompletedTask;
        }
    }
}