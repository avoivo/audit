using Microsoft.Extensions.Logging;
using Probanx.TransactionAudit.Core.Services;
using Probanx.TransactionAudit.ElasticSearch;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddElasticSearch(this IServiceCollection serviceCollection, string hostUrl, string index)
        {
            return serviceCollection
                .AddSingleton(_ => new TransactionStoreOptions(hostUrl, index))
                .AddTransient<ITransactionStore, TransactionStore>();
        }

    }
}
