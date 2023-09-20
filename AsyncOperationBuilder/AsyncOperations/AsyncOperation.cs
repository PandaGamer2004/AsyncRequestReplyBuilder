using System;
using System.Threading;
using System.Threading.Tasks;
using AsyncOperationBuilder.AsyncOperations.Exceptions;
using AsyncOperationBuilder.AsyncOperations.Interfaces;
using AsyncOperationBuilder.AsyncOperations.Models;
using AsyncOperationBuilder.AsyncOperations.StateMachines;
using Deep.Logic.Repositories.AsyncOperations;
using Deep.Logic.Repositories.AsyncOperations.Models;
using Umbraco.Core.Logging;

namespace AsyncOperationBuilder.AsyncOperations;

public class AsyncOperation<TAsyncOperationInitData, TAsyncOperationContext, TResult>
    : IAsyncOperation<TAsyncOperationInitData, TResult>
{
    private readonly ILogger logger;
    private readonly IAsyncOperationStateMachineFactory<TAsyncOperationInitData, TResult> stateMachineFactory;
    private readonly IAsyncOperationRepository<TAsyncOperationContext, TResult> asyncOperationRepository;
    
    //Nullable
    private AsyncOperationTicket asyncOperationTicket;

    public AsyncOperation(
        ILogger logger,
        IAsyncOperationStateMachineFactory<TAsyncOperationInitData, TResult> stateMachineFactory,
        IAsyncOperationRepository<TAsyncOperationContext, TResult> asyncOperationRepository,
        AsyncOperationTicket ticket)
    {
        this.logger = logger;
        this.stateMachineFactory = stateMachineFactory;
        this.asyncOperationRepository = asyncOperationRepository;
        this.asyncOperationTicket = ticket;
    }

    public Task<AsyncOperationTicket> Start(TAsyncOperationInitData asyncOperationData, CancellationToken ct = default)
        => RunBoundToAsyncState(() => StartWhenNotInitialized(asyncOperationData, ct), ct);
    private async Task<AsyncOperationTicket> RunBoundToAsyncState(
            Func<Task<AsyncOperationTicket>> runActualOperation,
            CancellationToken ct)
        {
            if (asyncOperationTicket is not null)
            {
                try
                {
                    AsyncOperationStateMachinePersistedState state = await asyncOperationRepository
                        .LoadAsyncOperationState(asyncOperationTicket, ct);

                    return new AsyncOperationTicket
                    {
                        Id = state.OperationTicket.Id,
                    };
                }
                catch (Exception ex)
                {
                    logger.Error<AsyncOperation<TAsyncOperationInitData, TAsyncOperationContext, TResult>>(ex.Message, ex);
                    throw new FailedToGetOperationStatus($"Failed to load state data for ticket id: {asyncOperationTicket}");
                }

            }
            return await runActualOperation();
        }
    private async Task<AsyncOperationTicket> StartWhenNotInitialized(TAsyncOperationInitData asyncOperationInitData,
        CancellationToken ct)
    {
        //Created new async operation
        AsyncOperationStateMachine<TResult> operationStateMachine = this.stateMachineFactory
            .CreateWithInitialState(asyncOperationInitData);
        while (operationStateMachine.GetOperationStatus() != AsyncOperationStatus.Started)
        {
            await operationStateMachine.PerformStateTransition(ct);
        }

        AsyncOperationTicket storeAsyncOperationState = await this.StoreAsyncOperationState(operationStateMachine, AsyncOperationTicket.NullObject);
        this.asyncOperationTicket = storeAsyncOperationState;
        return storeAsyncOperationState;
    }

    private Task<AsyncOperationTicket> StoreAsyncOperationState(
        AsyncOperationStateMachine<TResult> stateMachineStateToStore, AsyncOperationTicket operationTicket)
    {
        StatePersistenceModel statePersistenceModel = stateMachineStateToStore.GetPersistenceModel();
        var mappedState = new AsyncOperationStateMachinePersistedState
        {
            OperationTicket = operationTicket,
            OperationStatePayload = statePersistenceModel.StateData,
            OperationStatus = statePersistenceModel.OperationStatus
        };
        return asyncOperationRepository
            .StoreAsyncOperationState(mappedState);
    }
    

    public Task<AsyncOperationStatusResult<TResult>> GetStatus(CancellationToken ct = default)
    {
        this.ValidatePreviewTicket();
        return this.CheckOperationStatus(ct);
    }

    private void ValidatePreviewTicket()
    {
        if (asyncOperationTicket is null)
        {
            throw new FailedToGetOperationStatus("Async operation ticket not attached to operation");
        }
    }


    private async Task<AsyncOperationStatusResult<TResult>> CheckOperationStatus(CancellationToken ct)
    {

        var asyncOperationState = await this.asyncOperationRepository
            .LoadAsyncOperationState(asyncOperationTicket, ct);
        var persistenceModel = new StatePersistenceModel(asyncOperationState.OperationStatus,
            asyncOperationState.OperationStatePayload);
        AsyncOperationStateMachine<TResult> stateMachine = 
            this.stateMachineFactory.RestoreFromPersistence(persistenceModel);
        await stateMachine.PerformStateTransition(ct);
        return stateMachine.GetPresentationResult();
    }
    
}