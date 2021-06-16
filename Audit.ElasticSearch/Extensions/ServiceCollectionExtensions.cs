using Microsoft.Extensions.Logging;
using Audit.Core.Services;
using Audit.ElasticSearch;

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
