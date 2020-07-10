using Probanx.TransactionAudit.Core.Services;
using Probanx.TransactionAudit.Web.Services;
using RabbitMQ.Client;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRabbitMQ(this IServiceCollection serviceCollection, string hostName)
        {
            return serviceCollection
                .AddSingleton<IConnectionFactory>((_) => new ConnectionFactory() { HostName = hostName });
        }
        public static IServiceCollection AddMessageDispatcher<T>(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IMessageDispatcher<T>, RabbitMqMessageDispatcher<T>>();
        }

        public static IServiceCollection AddMessageConsumer<T>(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IMessageConsumer<T>, RabbitMqMessageConsumer<T>>();
        }

    }
}
