using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nest;
using Audit.Core.Models;
using Audit.Core.Services;

namespace Audit.ElasticSearch
{
    public class TransactionStore : ITransactionStore
    {
        private readonly ILogger _logger;
        private readonly IElasticClient _client;

        public TransactionStore(ILogger<TransactionStore> logger, TransactionStoreOptions options)
        {
            _logger = logger;
            var settings = new ConnectionSettings(new Uri(options.HostUrl))
                .DefaultIndex(options.Index);

            _client = new ElasticClient(settings);
        }

        public Task Insert(Message message)
        {
            _logger.LogTrace("Insert() enter");
            _logger.LogDebug("Insert({MESSAGE})", message);

            _client.IndexDocument(message);

            _logger.LogTrace("Insert() exit");
            return Task.CompletedTask;
        }

        public async Task<decimal> GetTotalAmount()
        {
            _logger.LogTrace("GetTotalAmount() enter");
            _logger.LogDebug("GetTotalAmount()");

            var result = await _client.SearchAsync<Message>(s => s.Aggregations(aggs => aggs.Sum("total_amount", sm => sm.Field(p => p.Value))));

            if (result.IsValid == false)
            {
                return default;
            }

            var sum = result.Aggregations.Sum("total_amount").Value;

            _logger.LogTrace("GetTotalAmount() exit");

            if (sum.HasValue)
            {
                return (decimal)sum.Value;
            }

            return default;

        }
    }
}
