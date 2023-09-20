using System.Threading;
using System.Threading.Tasks;

namespace AsyncOperationBuilder.AsyncOperations.Interfaces.AsyncOperationLifecycle;

public interface IAsyncIntergrationOperationFetchStrategy<TAsyncOperationContext, TStatusFetchResult>
{
    public Task<TStatusFetchResult> CheckStatus(TAsyncOperationContext context, CancellationToken ct = default); 
}

