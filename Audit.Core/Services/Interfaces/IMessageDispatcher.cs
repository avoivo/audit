using System.Threading.Tasks;

namespace Audit.Core.Services
{
    public interface IMessageDispatcher<T>
    {
        Task Dispatch(T message);

    }
}