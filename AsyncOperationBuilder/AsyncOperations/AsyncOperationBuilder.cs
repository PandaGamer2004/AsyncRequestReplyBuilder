using System.Linq;
using AsyncOperationBuilder.AsyncOperations.Exceptions;
using AsyncOperationBuilder.AsyncOperations.Interfaces;
using AsyncOperationBuilder.AsyncOperations.Interfaces.AsyncOperationLifecycle;
using AsyncOperationBuilder.AsyncOperations.Models;
using Deep.Logic.Repositories.AsyncOperations;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;

namespace AsyncOperationBuilder.AsyncOperations;

public class AsyncOperationBuilder<TAsyncOperationInitData, 
    TAsyncOperationContext,
    TStatusFetchResult> : IAsyncOperationBuilder<TAsyncOperationInitData, 
    TAsyncOperationContext,
    TStatusFetchResult>
{
    private readonly IFactory factory;


    public AsyncOperationBuilder(IFactory factory)
    {
        this.factory = factory;
    }
    //This 3 mandatory to initialize
    private IAsyncIntegrationOperationStarter<TAsyncOperationInitData, TAsyncOperationContext> asyncIntegrationOperationStarter;
    private IAsyncIntergrationOperationFetchStrategy<TAsyncOperationContext,TStatusFetchResult> intergrationOperationFetchStrategy;
    private IAsyncOperationSuccessResolver<TAsyncOperationContext,TStatusFetchResult> succesStatusResolver;

    //Can be null on start of the buil
    private AsyncOperationTicket associatedOperationTicket;


    public void SetAsyncOperationStartStrategy(IAsyncIntegrationOperationStarter<TAsyncOperationInitData, TAsyncOperationContext> asyncIntegrationOperationStarter)
    {
        this.asyncIntegrationOperationStarter = asyncIntegrationOperationStarter;
    }

    public void SetAsyncOperationStatusFetchStrategy(IAsyncIntergrationOperationFetchStrategy<TAsyncOperationContext, TStatusFetchResult> intergrationOperationFetchStrategy)
    {
        this.intergrationOperationFetchStrategy = intergrationOperationFetchStrategy;
    }

    public void SetSuccessOperationStatusResolver(IAsyncOperationSuccessResolver<TAsyncOperationContext, TStatusFetchResult> statusResolver)
    {
        this.succesStatusResolver = statusResolver;
    }
    
    public void SetInitPreviewTicket(AsyncOperationTicket operationTicket)
    {
        this.associatedOperationTicket = operationTicket;
    }
    private class InitializationStep
    {
        public string Name { get; set; }

        public object Operation { get; set; }
    }
    public IAsyncOperation<TAsyncOperationInitData, TStatusFetchResult> Build()
    {
        var requiredSteps = new InitializationStep[]
        {
            new() {Name = nameof(SetAsyncOperationStartStrategy), Operation = asyncIntegrationOperationStarter},
            new() {Name = nameof(SetAsyncOperationStatusFetchStrategy), Operation = intergrationOperationFetchStrategy},
            new() {Name = nameof(SetSuccessOperationStatusResolver), Operation = succesStatusResolver}
            };
        var notInitializedSteps = requiredSteps.Where(step => step.Operation is null).ToList();
        if (notInitializedSteps.Count != 0)
        {
            throw new IncompleteAsyncOperationBuildException("Failed to build async operation",
                requiredSteps.Select(step => step.Name));
        }

        ILogger operationLogger = (ILogger)factory.GetInstance(typeof(ILogger));
        IAsyncOperationStateMachineFactory<TAsyncOperationInitData, TStatusFetchResult> stateMachineFactory = 
            (IAsyncOperationStateMachineFactory<TAsyncOperationInitData, TStatusFetchResult>)
            factory.GetInstance(typeof(IAsyncOperationStateMachineFactory<TAsyncOperationInitData, TStatusFetchResult>));
        
        IAsyncOperationRepository<TAsyncOperationContext, TStatusFetchResult> asyncOperationRepository =
            (IAsyncOperationRepository<TAsyncOperationContext, TStatusFetchResult>)factory.GetInstance(
                typeof(IAsyncOperationRepository<TAsyncOperationContext, TStatusFetchResult>));
        
        return new AsyncOperation<TAsyncOperationInitData, TAsyncOperationContext, TStatusFetchResult>(
            operationLogger,
            stateMachineFactory,
            asyncOperationRepository,
            associatedOperationTicket
        );
    }
}



