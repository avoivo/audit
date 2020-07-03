using System.Threading.Tasks;
using Probanx.TransactionAudit.Core.Models;

namespace Probanx.TransactionAudit.Core.Services
{
    public interface IMessageDispatcher
    {
        Task Dispatch(Message message);

    }
}