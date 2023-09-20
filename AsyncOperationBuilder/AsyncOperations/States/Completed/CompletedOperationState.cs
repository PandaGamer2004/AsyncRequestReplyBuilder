using System.Threading;
using System.Threading.Tasks;
using AsyncOperationBuilder.AsyncOperations.Attributes;
using AsyncOperationBuilder.AsyncOperations.Interfaces.State;
using AsyncOperationBuilder.AsyncOperations.Models;
using AsyncOperationBuilder.AsyncOperations.Serialization.Interfaces;
using AsyncOperationBuilder.AsyncOperations.States.Base;

namespace AsyncOperationBuilder.AsyncOperations.States.Completed;

public class CompletedOperationState<TResultPayload>: BasePersistedAsyncState<CompletedOperationStatePersistenceModel<TResultPayload>>,
    IAsyncOperationAsyncState<TResultPayload>
{
    private TResultPayload result;

    private const AsyncOperationStatus OperationStatus = AsyncOperationStatus.Completed;
    //Final state
    public Task PerformStateOperation(IAsyncStateRoot<TResultPayload> stateRoot, CancellationToken ct)
        => Task.CompletedTask;

    public AsyncOperationStatusResult<TResultPayload> GetPresentationResult()
        =>  AsyncOperationStatusResult<TResultPayload>
            .CreateNotErrored(result, OperationStatus.ToString());

    public bool IsFinal => true;

    public AsyncOperationStatus GetOperationStatus()
        => OperationStatus;

    [DependencyOnlyStateConstructor]
    public CompletedOperationState(
        ISerializationStrategyProvider serializationStrategyProvider,
        AsyncOperationStatus operationStatus):
            base(serializationStrategyProvider, operationStatus)
    {
    }

    public CompletedOperationState(
        TResultPayload result,
        ISerializationStrategyProvider serializationStrategyProvider):
        base(serializationStrategyProvider, OperationStatus)
    {
        this.result = result;
    }



    protected override void ApplyPersistedModel(
        CompletedOperationStatePersistenceModel<TResultPayload> persistedStateModel)
    {
        this.result = persistedStateModel.OperationResult;
    }

    protected override CompletedOperationStatePersistenceModel<TResultPayload> AccessPersistedModel()
        => new CompletedOperationStatePersistenceModel<TResultPayload>()
        {
            OperationResult = result
        };
}