using System;
using System.Collections.Generic;
using AsyncOperationBuilder.AsyncOperations.Exceptions;
using AsyncOperationBuilder.AsyncOperations.Interfaces;
using AsyncOperationBuilder.AsyncOperations.Interfaces.State;
using AsyncOperationBuilder.AsyncOperations.Models;
using AsyncOperationBuilder.AsyncOperations.Serialization.Interfaces;
using AsyncOperationBuilder.AsyncOperations.StateMachines;
using AsyncOperationBuilder.AsyncOperations.States.GoingToStartState;
using AsyncOperationBuilder.Serialization.SerializationProviders.Json.Helpers;
using Umbraco.Core.Composing;

namespace AsyncOperationBuilder.AsyncOperations.Factories;

public class AsyncOperationStateMachineFactory<TInitialData, TContext, TOperationResult>:
    IAsyncOperationStateMachineFactory<TInitialData, TOperationResult>
{
    //TODO register for operationState
    private readonly IIntegrationsFactory<TInitialData, TContext, TOperationResult> integrationsFactory;
    private readonly ISerializationStrategyProvider serializationStrategyProvider;
    private readonly List<Func<IAsyncOperationAsyncState<TOperationResult>>> persistentOperationStateFactories;
    public AsyncOperationStateMachineFactory(
        IIntegrationsFactory<TInitialData, TContext, TOperationResult> integrationsFactory,
        ISerializationStrategyProvider serializationStrategyProvider,
        IFactory dependencyProvider
    )
    {
        this.integrationsFactory = integrationsFactory;
        this.serializationStrategyProvider = serializationStrategyProvider;
    }

    public AsyncOperationStateMachine<TOperationResult> CreateWithInitialState(TInitialData initialData)
    {
        var state = new GoingToStartOperationAsyncState<TInitialData, TContext, TOperationResult>(
            initialData,
            integrationsFactory,
            serializationStrategyProvider);
        return new AsyncOperationStateMachine<TOperationResult>(
            state
        );
    }

    public AsyncOperationStateMachine<TOperationResult> RestoreFromPersistence(StatePersistenceModel statePersistenceModel)
    {
        foreach (var pendingStateFactory in persistentOperationStateFactories)
        {
            var state = pendingStateFactory();
            if (state.TryApplyPersistenceModel(statePersistenceModel))
            {
                return new AsyncOperationStateMachine<TOperationResult>(state);
            }
        }
        var serializedPersistenceModel = JsonProviderFactory
            .CreateIgnoreCaseSerializationProvider<StatePersistenceModel>()
            .Serialize(statePersistenceModel);

        throw new FailedToGetAsyncOperationStateFromPersistence(
            $"Failed to provide state from persistence model{Environment.NewLine}" +
                   $"Passed persistence model: {serializedPersistenceModel}");
    }
}