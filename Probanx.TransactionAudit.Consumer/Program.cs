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

        const string ELASTIC_HOST_URL = "ELASTIC_HOST_URL";
        const string ELASTIC_INDEX = "ELASTIC_INDEX";
        const string RABBIT_MQ_HOST_NAME = "RABBIT_MQ_HOST_NAME";

        static void Main(string[] args)
        {
            //should wait for rabbitmq to start
            Thread.Sleep(30000);

            string elasticHostUrl = Environment.GetEnvironmentVariable(ELASTIC_HOST_URL) ?? "http://host.docker.internal:9200";
            string elasticIndex = Environment.GetEnvironmentVariable(ELASTIC_INDEX) ?? "transactions";
            string rabbitMqHostName = Environment.GetEnvironmentVariable(RABBIT_MQ_HOST_NAME) ?? "host.docker.internal";

            var store = new TransactionStore(elasticHostUrl, elasticIndex);

            var factory = new ConnectionFactory() { HostName = rabbitMqHostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "Message",
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
                channel.BasicConsume(queue: "Message",
                                    autoAck: true,
                                    consumer: consumer);

                // Console.WriteLine(" Press [enter] to exit.");
                // Console.ReadLine();
                Thread.Sleep(Timeout.Infinite);


            }
        }
    }
}
