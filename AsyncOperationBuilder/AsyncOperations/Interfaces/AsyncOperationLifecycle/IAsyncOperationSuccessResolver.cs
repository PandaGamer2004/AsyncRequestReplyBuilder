using AsyncOperationBuilder.AsyncOperations.Models;

namespace AsyncOperationBuilder.AsyncOperations.Interfaces.AsyncOperationLifecycle;


public interface IAsyncOperationSuccessResolver<TAsyncOperationContext, TStatusFetchResult>
{
    public AsyncOperationSuccessResolverResult Resolve(
        StateResolverPayload<TAsyncOperationContext, TStatusFetchResult> statusFetchResult);
}
