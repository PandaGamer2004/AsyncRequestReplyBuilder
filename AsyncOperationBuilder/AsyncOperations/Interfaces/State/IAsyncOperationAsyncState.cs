using System.Threading;
using System.Threading.Tasks;
using AsyncOperationBuilder.AsyncOperations.Models;

namespace AsyncOperationBuilder.AsyncOperations.Interfaces.State;

public interface IAsyncOperationAsyncState<TPayload>: IPersistedAsyncState
{
    public Task PerformStateOperation(
        IAsyncStateRoot<TPayload> stateRoot,
        CancellationToken ct
    );
    public AsyncOperationStatusResult<TPayload> GetPresentationResult();

    public bool IsFinal { get; }
    AsyncOperationStatus GetOperationStatus();
}