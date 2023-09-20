using AsyncOperationBuilder.AsyncOperations.Interfaces.AsyncOperationLifecycle;

namespace AsyncOperationBuilder.AsyncOperations.Interfaces;

public interface IIntegrationsFactory<TInitialData, TOperationContext, TResult>:
    IStatusCheckIntegrationsFactory<TOperationContext, TResult>
{
    IAsyncIntegrationOperationStarter<TInitialData, TOperationContext>
        CreateOperationStarter();
}