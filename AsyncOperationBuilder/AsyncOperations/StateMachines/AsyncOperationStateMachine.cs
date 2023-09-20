using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AsyncOperationBuilder.AsyncOperations.Exceptions;
using AsyncOperationBuilder.AsyncOperations.Interfaces;
using AsyncOperationBuilder.AsyncOperations.Interfaces.State;
using AsyncOperationBuilder.AsyncOperations.Models;
using AsyncOperationBuilder.Serialization.SerializationProviders.Json.Helpers;

namespace AsyncOperationBuilder.AsyncOperations.StateMachines;


public class AsyncOperationStateMachine<TOperationResult>:
    IAsyncOperationStateMachine,
    IAsyncStateRoot<TOperationResult>
{
    private IAsyncOperationAsyncState<TOperationResult> asyncOperationAsyncState;
    
    public AsyncOperationStateMachine(IAsyncOperationAsyncState<TOperationResult> asyncOperationAsyncState)
    {
        this.asyncOperationAsyncState = asyncOperationAsyncState;
    }
    public void SetState(IAsyncOperationAsyncState<TOperationResult> operationAsyncState)
    {
        asyncOperationAsyncState = operationAsyncState;
    }
    
    public static AsyncOperationStateMachine<TOperationResult> RestoreFrom(
        StatePersistenceModel storePersistenceModel, 
        IEnumerable<Func<IAsyncOperationAsyncState<TOperationResult>>> persistentOperationStateFactories)
    {
        foreach (var persistentOperationStateFactory 
                 in persistentOperationStateFactories)
        {
            IAsyncOperationAsyncState<TOperationResult> asyncOperationAsyncState =
                persistentOperationStateFactory();
            if (asyncOperationAsyncState.TryApplyPersistenceModel(storePersistenceModel))
            {
                return new AsyncOperationStateMachine<TOperationResult>(asyncOperationAsyncState);
            }
        }

        var stateModelSerializer = JsonProviderFactory
            .CreateIgnoreCaseSerializationProvider<StatePersistenceModel>();
        string serializedStoreModel = stateModelSerializer
            .Serialize(storePersistenceModel);
        
        throw new FailedToGetAsyncOperationStateFromPersistence(
            $"Failed to restore async operation state from stored model{Environment.NewLine}" +
                   $"{serializedStoreModel}");
    }
    
    public Task PerformStateTransition(CancellationToken ct)
        => this.asyncOperationAsyncState.PerformStateOperation(this, ct);

    public StatePersistenceModel GetPersistenceModel()
        => asyncOperationAsyncState.GetPersistenceModel();

    public AsyncOperationStatusResult<TOperationResult> GetPresentationResult()
        => asyncOperationAsyncState.GetPresentationResult();
}