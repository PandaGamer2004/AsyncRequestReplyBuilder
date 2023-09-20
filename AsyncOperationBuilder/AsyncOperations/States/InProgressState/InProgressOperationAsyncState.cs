using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AsyncOperationBuilder.AsyncOperations.Attributes;
using AsyncOperationBuilder.AsyncOperations.Interfaces;
using AsyncOperationBuilder.AsyncOperations.Interfaces.AsyncOperationLifecycle;
using AsyncOperationBuilder.AsyncOperations.Interfaces.State;
using AsyncOperationBuilder.AsyncOperations.Models;
using AsyncOperationBuilder.AsyncOperations.Serialization.Interfaces;
using AsyncOperationBuilder.AsyncOperations.States.Base;
using AsyncOperationBuilder.AsyncOperations.States.Completed;
using AsyncOperationBuilder.AsyncOperations.States.Failed;

namespace AsyncOperationBuilder.AsyncOperations.States.InProgressState;

public class InProgressOperationAsyncState<TOperationContext, TStatusCheckResult> 
    :BasePersistedAsyncState<InProgressStatePersistenceModel<TOperationContext, TStatusCheckResult>>, 
        IAsyncOperationAsyncState<TStatusCheckResult>
{
    private readonly IStatusCheckIntegrationsFactory<TOperationContext, TStatusCheckResult> integrationsFactory;
    private TOperationContext operationContext;
    private const AsyncOperationStatus AsyncOperationStatus = Models.AsyncOperationStatus.InProgress;
    
    //TODO made this way for simplicity
    private List<TStatusCheckResult> statusCheckResults = new List<TStatusCheckResult>();

    [DependencyOnlyStateConstructor]
    public InProgressOperationAsyncState(
        ISerializationStrategyProvider serializationStrategyProvider,
        IStatusCheckIntegrationsFactory<TOperationContext, TStatusCheckResult> integrationsFactory) 
        : base(serializationStrategyProvider, AsyncOperationStatus)
    {
        this.integrationsFactory = integrationsFactory;
    }
    
    public InProgressOperationAsyncState(
        ISerializationStrategyProvider serializationStrategyProvider,
        IStatusCheckIntegrationsFactory<TOperationContext, TStatusCheckResult> integrationsFactory,
        TOperationContext operationContext) : this(serializationStrategyProvider, integrationsFactory)
    {
        this.operationContext = operationContext;
    }
    
    public async Task PerformStateOperation(IAsyncStateRoot<TStatusCheckResult> stateRoot, CancellationToken ct)
    {
        try
        {
            var operationFetchStrategy =
                integrationsFactory.CreateOperationFetchStrategy();
            var operationStatusCheckResult = await operationFetchStrategy
                .CheckStatus(operationContext, ct);
            this.statusCheckResults.Add(operationStatusCheckResult);
            var successResolver =
                integrationsFactory.CreateSuccessResolver();
            var successResolverResult = successResolver.Resolve(new StateResolverPayload<TOperationContext, TStatusCheckResult>
            {
                AsyncOperationContext = this.operationContext,
                CheckResults = this.statusCheckResults
            });
            if (successResolverResult.IsCompleted)
            {
                var completedState = new CompletedOperationState<TStatusCheckResult>(
                    operationStatusCheckResult,
                    serializationStrategyProvider
                );
                stateRoot.SetState(completedState);
            }
        }
        catch (System.Exception ex)
        {
            var failedOperationState = new FailedOperationAsyncState<TStatusCheckResult>(
                new AsyncOperationError
                {
                    FacedException  = ex
                }, this.serializationStrategyProvider);
            stateRoot.SetState(failedOperationState);
        }
    }
    
    public AsyncOperationStatusResult<TStatusCheckResult> GetPresentationResult()
        => AsyncOperationStatusResult<TStatusCheckResult>
            .CreateNotErrored(
                this.statusCheckResults.Last(),
                AsyncOperationStatus.ToString()
                );

    public bool IsFinal => false;

    protected override void ApplyPersistedModel(InProgressStatePersistenceModel<TOperationContext, TStatusCheckResult> persistedStateModel)
    {
        this.operationContext = persistedStateModel.ContextData;
        this.statusCheckResults = persistedStateModel.StatusCheckResults.ToList();
    }

    
    protected override InProgressStatePersistenceModel<TOperationContext, TStatusCheckResult> AccessPersistedModel()
        => new InProgressStatePersistenceModel<TOperationContext, TStatusCheckResult>()
        {
            ContextData = this.operationContext,
            StatusCheckResults = this.statusCheckResults
        };
}