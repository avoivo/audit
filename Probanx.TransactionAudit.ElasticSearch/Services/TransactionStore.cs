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
    }
}
