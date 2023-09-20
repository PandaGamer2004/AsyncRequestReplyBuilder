using AsyncOperationBuilder.AsyncOperations.Models;
using AsyncOperationBuilder.AsyncOperations.StateMachines;

namespace AsyncOperationBuilder.AsyncOperations.Interfaces;

public interface IAsyncOperationStateMachineFactory<TInitialData, TOperationResult>
{
    AsyncOperationStateMachine<TOperationResult> CreateWithInitialState(TInitialData initialData);
    
    AsyncOperationStateMachine<TOperationResult> RestoreFromPersistence(StatePersistenceModel statePersistenceModel);
}