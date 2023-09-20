using System.Threading;
using System.Threading.Tasks;
using AsyncOperationBuilder.AsyncOperations.Attributes;
using AsyncOperationBuilder.AsyncOperations.Interfaces;
using AsyncOperationBuilder.AsyncOperations.Interfaces.AsyncOperationLifecycle;
using AsyncOperationBuilder.AsyncOperations.Interfaces.State;
using AsyncOperationBuilder.AsyncOperations.Models;
using AsyncOperationBuilder.AsyncOperations.Serialization.Interfaces;
using AsyncOperationBuilder.AsyncOperations.States.Base;
using AsyncOperationBuilder.AsyncOperations.States.Failed;
using AsyncOperationBuilder.AsyncOperations.States.Started;

namespace AsyncOperationBuilder.AsyncOperations.States.GoingToStartState;


public class GoingToStartOperationAsyncState<TOperationStartData, TOperationContext, TStatusCheckResult>:
    BasePersistedAsyncState<GoingToStartPersistenceStateData<TOperationStartData>>,
    IAsyncOperationAsyncState<TStatusCheckResult>
{
    //Transient dependencies
    private readonly IIntegrationsFactory<TOperationStartData, TOperationContext, TStatusCheckResult> integrationsFactory;
    private readonly ISerializationStrategyProvider serializationStrategyProvider;
    private const AsyncOperationStatus StateAssociatedStatus = AsyncOperationStatus.GoingToStart;
    
    //Mutable could be restored from state
    private TOperationStartData operationInitialData;

    [DependencyOnlyStateConstructor]
    public GoingToStartOperationAsyncState(
        IIntegrationsFactory<TOperationStartData, TOperationContext, TStatusCheckResult> integrationsFactory,
        ISerializationStrategyProvider serializationStrategyProvider): base(serializationStrategyProvider, StateAssociatedStatus)
    {
        this.integrationsFactory = integrationsFactory;
        this.serializationStrategyProvider = serializationStrategyProvider;
    }
    
    public GoingToStartOperationAsyncState(
        TOperationStartData operationInitialData,
        IIntegrationsFactory<TOperationStartData, TOperationContext, TStatusCheckResult> integrationsFactory,
        ISerializationStrategyProvider serializationStrategyProvider
    ): this(integrationsFactory, serializationStrategyProvider)
    {
        this.operationInitialData = operationInitialData;
    }

    public async Task PerformStateOperation(IAsyncStateRoot<TStatusCheckResult> stateRoot, CancellationToken ct)
    {
        try
        {
            var asyncOperationStarter = integrationsFactory.CreateOperationStarter();
            TOperationContext asyncOperationContext = await asyncOperationStarter
                .StartAsyncOperation(operationInitialData, ct);
            stateRoot.SetState(new StartedOperationAsyncState<TOperationContext, TStatusCheckResult>(
                asyncOperationContext,
                integrationsFactory,
                serializationStrategyProvider
                ));
        }
        catch (System.Exception ex)
        {
            stateRoot.SetState(
                new FailedOperationAsyncState<TStatusCheckResult>(new AsyncOperationError
            {
                FacedException = ex
            }, serializationStrategyProvider));
        }
    }
    public AsyncOperationStatusResult<TStatusCheckResult> GetPresentationResult()
            => AsyncOperationStatusResult<TStatusCheckResult>.CreateNotErrored(
                default(TStatusCheckResult),
                        StateAssociatedStatus.ToString());

    public bool IsFinal => false;

    public AsyncOperationStatus GetOperationStatus()
        => StateAssociatedStatus;

    protected override void ApplyPersistedModel(GoingToStartPersistenceStateData<TOperationStartData> persistedStateModel)
    {
        this.operationInitialData = persistedStateModel.InitialData;
    }

    protected override GoingToStartPersistenceStateData<TOperationStartData> AccessPersistedModel()
        => new GoingToStartPersistenceStateData<TOperationStartData>()
        {
            InitialData = operationInitialData
        };
}
