using AsyncOperationBuilder.AsyncOperations.Interfaces;
using AsyncOperationBuilder.AsyncOperations.Interfaces.AsyncOperationLifecycle;
using Umbraco.Core.Composing;

namespace AsyncOperationBuilder.AsyncOperations.Factories;

//Factory that composes all dependencies to deny injection by separate instance
public class UmbracoComposingIntegrationsFactory<TInitialData, TOperationContext, TResult>:
    IIntegrationsFactory<TInitialData, TOperationContext, TResult>
{
    private readonly IFactory factory;

    public UmbracoComposingIntegrationsFactory(IFactory factory)
    {
        this.factory = factory;
    }

    public IAsyncIntegrationOperationStarter<TInitialData, TOperationContext> CreateOperationStarter()
        => (IAsyncIntegrationOperationStarter<TInitialData, TOperationContext>)
            factory.GetInstance(typeof(IAsyncIntegrationOperationStarter<TInitialData, TOperationContext>));

    public IAsyncIntergrationOperationFetchStrategy<TOperationContext, TResult> CreateOperationFetchStrategy()
        => (IAsyncIntergrationOperationFetchStrategy<TOperationContext, TResult>)
            factory.GetInstance(typeof(IAsyncIntergrationOperationFetchStrategy<TOperationContext, TResult>));

    public IAsyncOperationSuccessResolver<TOperationContext, TResult> CreateSuccessResolver()
        => (IAsyncOperationSuccessResolver<TOperationContext, TResult>)
            factory.GetInstance(typeof(IAsyncOperationSuccessResolver<TOperationContext, TResult>));
}