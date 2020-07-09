using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Probanx.TransactionAudit.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            //should wait for rabbitmq to start
            Thread.Sleep(30000);

            var factory = new ConnectionFactory() { HostName = "host.docker.internal" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);
                };
                channel.BasicConsume(queue: "hello",
                                    autoAck: true,
                                    consumer: consumer);

                // Console.WriteLine(" Press [enter] to exit.");
                // Console.ReadLine();
                Thread.Sleep(Timeout.Infinite);


            }
        }
    }
}
