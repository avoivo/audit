using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Probanx.TransactionAudit.Core.Models;
using Probanx.TransactionAudit.Core.Services;

namespace Probanx.TransactionAudit.Consumer
{
    class Program
    {

        const string ELASTIC_HOST_URL = "ELASTIC_HOST_URL";
        const string ELASTIC_INDEX = "ELASTIC_INDEX";
        const string RABBIT_MQ_HOST_NAME = "RABBIT_MQ_HOST_NAME";

        static void Main(string[] args)
        {
            string elasticHostUrl = Environment.GetEnvironmentVariable(ELASTIC_HOST_URL) ?? "http://host.docker.internal:9200";
            string elasticIndex = Environment.GetEnvironmentVariable(ELASTIC_INDEX) ?? "transactions";
            string rabbitMqHostName = Environment.GetEnvironmentVariable(RABBIT_MQ_HOST_NAME) ?? "host.docker.internal";

            var serviceCollection = new ServiceCollection();
            var serviceProvider = serviceCollection
                .AddRabbitMQ(rabbitMqHostName)
                .AddMessageConsumer<Message>()
                .AddElasticSearch(elasticHostUrl, elasticIndex)
                .AddLogging(loggingBuilder => loggingBuilder.AddConsole())
                .BuildServiceProvider();


            //should wait for rabbitmq to start
            Thread.Sleep(30000);

            var messageConsumer = serviceProvider.GetService<IMessageConsumer<Message>>();
            var store = serviceProvider.GetService<ITransactionStore>();

            messageConsumer.Consume(m => store.Insert(m).Wait());

        }
    }
}