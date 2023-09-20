using System.Threading;
using System.Threading.Tasks;

namespace AsyncOperationBuilder.AsyncOperations.Interfaces.AsyncOperationLifecycle;

public interface IAsyncIntegrationOperationStarter<TAsyncOperationInitData, TAsyncOperationContext>
{
    public Task<TAsyncOperationContext> StartAsyncOperation(TAsyncOperationInitData input, CancellationToken ct = default);
}
