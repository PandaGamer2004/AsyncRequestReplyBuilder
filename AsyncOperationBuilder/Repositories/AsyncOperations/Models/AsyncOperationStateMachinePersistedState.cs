using AsyncOperationBuilder.AsyncOperations.Models;

namespace Deep.Logic.Repositories.AsyncOperations.Models;

public class AsyncOperationStateMachinePersistedState
{
    public AsyncOperationTicket OperationTicket { get; set; }
    
    public string OperationStatePayload { get; set; }

    public AsyncOperationStatus OperationStatus { get; set; }
}