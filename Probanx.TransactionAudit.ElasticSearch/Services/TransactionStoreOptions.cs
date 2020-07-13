namespace Probanx.TransactionAudit.ElasticSearch
{
    public class TransactionStoreOptions
    {
        public TransactionStoreOptions(string hostUrl, string index)
        {
            HostUrl = hostUrl;
            Index = index;
        }
        public string HostUrl { get; private set; }
        public string Index { get; private set; }
    }
}