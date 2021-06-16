using System.Threading.Tasks;
using Audit.Core.Models;

namespace Audit.Core.Services
{
    public interface ITransactionStore
    {
        Task Insert(Message message);
        Task<decimal> GetTotalAmount();
    }
}