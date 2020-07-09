using System;
using System.Threading.Tasks;
using Nest;
using Probanx.TransactionAudit.Core.Models;
using Probanx.TransactionAudit.Core.Services;

namespace Probanx.TransactionAudit.ElasticSearch
{
    public class TransactionStore : ITransactionStore
    {
        private readonly IElasticClient client;
        public TransactionStore()
        {
            var settings = new ConnectionSettings(new Uri("http://host.docker.internal:9200"))
                .DefaultIndex("people");

            client = new ElasticClient(settings);
        }

        public Task Insert(Message message)
        {
            client.IndexDocument(message);
            return Task.CompletedTask;
        }

        public async Task<decimal> GetTotalAmount()
        {
            var result = await client.SearchAsync<Message>(s => s.Aggregations(aggs => aggs.Sum("total_amount", sm => sm.Field(p => p.Value))));

            var sum = result.Aggregations.Sum("total_amount").Value;

            if (sum.HasValue)
            {
                return (decimal)sum.Value;
            }

            return default;

        }
    }
}
