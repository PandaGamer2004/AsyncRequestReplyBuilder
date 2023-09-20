using System.Threading;
using System.Threading.Tasks;
using AsyncOperationBuilder.AsyncOperations.Attributes;
using AsyncOperationBuilder.AsyncOperations.Interfaces;
using AsyncOperationBuilder.AsyncOperations.Interfaces.State;
using AsyncOperationBuilder.AsyncOperations.Models;
using AsyncOperationBuilder.AsyncOperations.Serialization.Interfaces;
using AsyncOperationBuilder.AsyncOperations.States.Base;
using AsyncOperationBuilder.AsyncOperations.States.InProgressState;

namespace AsyncOperationBuilder.AsyncOperations.States.Started;


public class StartedOperationAsyncState<TContextData, TStatusCheckResult>:
    BasePersistedAsyncState<StartedOperationStateModel<TContextData>>,
    IAsyncOperationAsyncState<TStatusCheckResult>
{
    private TContextData asyncOperationContext;
    private readonly IStatusCheckIntegrationsFactory<TContextData, TStatusCheckResult> integrationsFactory;
    private readonly ISerializationStrategyProvider serializationStrategyProvider;
    private const AsyncOperationStatus OperationStatus = AsyncOperationStatus.Started;

    
    [DependencyOnlyStateConstructor]
    public StartedOperationAsyncState(
        IStatusCheckIntegrationsFactory<TContextData, TStatusCheckResult> integrationsFactory,
        ISerializationStrategyProvider serializationStrategyProvider):
            base(serializationStrategyProvider, OperationStatus)
    {
        this.integrationsFactory = integrationsFactory;
        this.serializationStrategyProvider = serializationStrategyProvider;
    }
    public StartedOperationAsyncState(
        TContextData asyncOperationContext,
        IStatusCheckIntegrationsFactory<TContextData, TStatusCheckResult> integrationsFactory,
        ISerializationStrategyProvider serializationStrategyProvider):
            this(integrationsFactory, serializationStrategyProvider)
    {
        this.asyncOperationContext = asyncOperationContext;
    }

    
    public Task PerformStateOperation(IAsyncStateRoot<TStatusCheckResult> stateRoot, CancellationToken ct)
    {
        //Just intermediate point in state transition pipeline
        stateRoot.SetState(
            new InProgressOperationAsyncState<TContextData,TStatusCheckResult>(
                serializationStrategyProvider,
                integrationsFactory,
                asyncOperationContext
            ));
        return Task.CompletedTask;
    }
    

    public AsyncOperationStatusResult<TStatusCheckResult> GetPresentationResult()
        => AsyncOperationStatusResult<TStatusCheckResult>.CreateNotErrored(
            default(TStatusCheckResult), OperationStatus.ToString());

    public bool IsFinal => false;

    protected override void ApplyPersistedModel(StartedOperationStateModel<TContextData> persistedStateModel)
    {
        this.asyncOperationContext = persistedStateModel.AsyncOperationContextData;
    }

    protected override StartedOperationStateModel<TContextData> AccessPersistedModel()
        => new StartedOperationStateModel<TContextData>()
        {
            AsyncOperationContextData = asyncOperationContext
        };
}