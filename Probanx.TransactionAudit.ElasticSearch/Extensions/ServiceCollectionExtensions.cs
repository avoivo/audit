using Probanx.TransactionAudit.Core.Services;
using Probanx.TransactionAudit.ElasticSearch;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddElasticSearch(this IServiceCollection serviceCollection, string hostUrl, string index)
        {
            return serviceCollection
                .AddTransient<ITransactionStore>(_ => new TransactionStore(hostUrl, index));
        }

    }
}
