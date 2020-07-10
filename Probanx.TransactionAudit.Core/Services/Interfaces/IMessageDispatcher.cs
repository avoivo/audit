using System.Threading.Tasks;
using Probanx.TransactionAudit.Core.Models;

namespace Probanx.TransactionAudit.Core.Services
{
    public interface IMessageDispatcher<T>
    {
        Task Dispatch(T message);

    }
}