using System;
using System.Threading.Tasks;

namespace Audit.Core.Services
{
    public interface IMessageConsumer<T>
    {
        Task Consume(Action<T> consumeAction);

    }
}