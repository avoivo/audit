using System;
using System.Threading.Tasks;

namespace Probanx.TransactionAudit.Core.Services
{
    public interface IMessageConsumer<T>
    {
        Task Consume(Action<T> consumeAction);

    }
}