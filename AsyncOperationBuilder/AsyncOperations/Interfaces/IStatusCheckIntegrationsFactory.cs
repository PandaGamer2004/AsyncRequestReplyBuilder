using AsyncOperationBuilder.AsyncOperations.Interfaces.AsyncOperationLifecycle;

namespace AsyncOperationBuilder.AsyncOperations.Interfaces;

public interface IStatusCheckIntegrationsFactory<TOperationContext, TResult>
{
    IAsyncIntergrationOperationFetchStrategy<TOperationContext, TResult> 
        CreateOperationFetchStrategy();

    IAsyncOperationSuccessResolver<TOperationContext, TResult> 
        CreateSuccessResolver();
    
    
}
