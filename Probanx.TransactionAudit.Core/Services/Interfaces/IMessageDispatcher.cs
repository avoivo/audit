using System.Threading.Tasks;

namespace Probanx.TransactionAudit.Core.Services
{
    public interface IMessageDispatcher<T>
    {
        Task Dispatch(T message);

    }
}