using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using Probanx.TransactionAudit.Core.Models;
using Probanx.TransactionAudit.ElasticSearch;
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

            var store = new TransactionStore();

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



                    try
                    {
                        Console.WriteLine("inserting to store");
                        var messageAsJson = JsonSerializer.Deserialize<Message>(message);
                        store.Insert(messageAsJson).Wait();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("exception thrown");
                        Console.WriteLine(ex);
                    }

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
