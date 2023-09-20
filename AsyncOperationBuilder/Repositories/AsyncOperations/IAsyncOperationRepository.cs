using System.Threading;
using System.Threading.Tasks;
using AsyncOperationBuilder.AsyncOperations.Models;
using Deep.Logic.Repositories.AsyncOperations.Models;

namespace Deep.Logic.Repositories.AsyncOperations;


public interface IAsyncOperationRepository<TContextData, TOperationResult>
{
    public Task<AsyncOperationTicket> StoreAsyncOperationState(
        AsyncOperationStateMachinePersistedState asyncOperationStateMachine, 
        CancellationToken ct = default
        );
    
    public Task<AsyncOperationStateMachinePersistedState> LoadAsyncOperationState(
        AsyncOperationTicket ticket,
        CancellationToken ct = default
        );
}