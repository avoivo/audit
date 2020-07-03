using Probanx.TransactionAudit.Core.Services;
using Probanx.TransactionAudit.Web.Services;
using RabbitMQ.Client;

namespace  Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddActiveMQ(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IConnectionFactory, ConnectionFactory>()
                .AddSingleton<IMessageDispatcher, RabbitMqMessageDispatcher>();
        }

    }
}