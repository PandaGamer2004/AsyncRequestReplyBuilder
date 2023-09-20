using AsyncOperationBuilder.AsyncOperations.Interfaces;
using AsyncOperationBuilder.AsyncOperations.Interfaces.AsyncOperationLifecycle;

namespace AsyncOperationBuilder.AsyncOperations;

public interface IAsyncOperationBuilder<
    TAsyncOperationInitData,
    TAsyncOperationContext,
    TStatusFetchResult> : IAsyncOperationPreviewTicketScopedBuilder<TAsyncOperationInitData, TStatusFetchResult>

{
    public void SetAsyncOperationStartStrategy(
        IAsyncIntegrationOperationStarter<TAsyncOperationInitData, TAsyncOperationContext> asyncIntegrationOperationStarter
    );

    public void SetAsyncOperationStatusFetchStrategy(
        IAsyncIntergrationOperationFetchStrategy<TAsyncOperationContext, TStatusFetchResult> intergrationOperationFetchStrategy
    );

    public void SetSuccessOperationStatusResolver(
        IAsyncOperationSuccessResolver<TAsyncOperationContext, TStatusFetchResult> statusResolver);
    
}


