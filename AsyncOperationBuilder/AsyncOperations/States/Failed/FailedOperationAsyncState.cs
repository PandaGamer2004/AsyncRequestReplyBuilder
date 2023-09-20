using System.Threading;
using System.Threading.Tasks;
using AsyncOperationBuilder.AsyncOperations.Attributes;
using AsyncOperationBuilder.AsyncOperations.Interfaces.AsyncOperationLifecycle;
using AsyncOperationBuilder.AsyncOperations.Interfaces.State;
using AsyncOperationBuilder.AsyncOperations.Models;
using AsyncOperationBuilder.AsyncOperations.Serialization.Interfaces;
using AsyncOperationBuilder.AsyncOperations.States.Base;

namespace AsyncOperationBuilder.AsyncOperations.States.Failed;

public class FailedOperationAsyncState<TResult>: BasePersistedAsyncState<FailedOperationAsyncPersistenceState>,
    IAsyncOperationAsyncState<TResult>
{
    private const AsyncOperationStatus OperationStatus = AsyncOperationStatus.Completed;
    private string errorMessage;
    
    
    [DependencyOnlyStateConstructor]
    public FailedOperationAsyncState(ISerializationStrategyProvider serializationStrategyProvider):
        base(serializationStrategyProvider, OperationStatus){ }
    
    public FailedOperationAsyncState(
        AsyncOperationError asyncOperationError, 
        ISerializationStrategyProvider serializationStrategyProvider): this(serializationStrategyProvider)
    {
        this.errorMessage = asyncOperationError.FacedException.Message;
    }

    //Final state
    public Task PerformStateOperation(IAsyncStateRoot<TResult> stateRoot, CancellationToken ct)
        => Task.CompletedTask;
    
    protected override void ApplyPersistedModel(FailedOperationAsyncPersistenceState persistedStateModel)
    {
        this.errorMessage = persistedStateModel.ErrorMessage;
    }
    protected override FailedOperationAsyncPersistenceState AccessPersistedModel()
        => new FailedOperationAsyncPersistenceState
        {
            ErrorMessage = errorMessage
        };

    public AsyncOperationStatusResult<TResult> GetPresentationResult()
        => AsyncOperationStatusResult<TResult>.CreateErrored(errorMessage, OperationStatus.ToString());

    public bool IsFinal => true;
    
}