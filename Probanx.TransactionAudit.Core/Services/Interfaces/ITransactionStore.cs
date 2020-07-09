using System.Threading.Tasks;
using Probanx.TransactionAudit.Core.Models;

namespace Probanx.TransactionAudit.Core.Services
{
    public interface ITransactionStore
    {
        Task Insert(Message message);
    }
}