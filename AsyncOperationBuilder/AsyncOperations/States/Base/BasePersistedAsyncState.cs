using AsyncOperationBuilder.AsyncOperations.Interfaces;
using AsyncOperationBuilder.AsyncOperations.Models;
using AsyncOperationBuilder.AsyncOperations.Serialization.Interfaces;

namespace AsyncOperationBuilder.AsyncOperations.States.Base;

public abstract class BasePersistedAsyncState<TPersistedStateModel>: IPersistedAsyncState
{
    protected readonly ISerializationStrategyProvider serializationStrategyProvider;
    private readonly AsyncOperationStatus operationStatus;
    
    internal BasePersistedAsyncState(
        ISerializationStrategyProvider serializationStrategyProvider,
        AsyncOperationStatus operationStatus)
    {
        this.operationStatus = operationStatus;
        this.serializationStrategyProvider = serializationStrategyProvider;
    }
    public StatePersistenceModel GetPersistenceModel()
    {
        var storedStateModel = AccessPersistedModel();
        var serializationStrategy = serializationStrategyProvider
            .CreateSerializationStrategy<TPersistedStateModel>();
        string serializedPersistenceModel = serializationStrategy
            .Serialize(storedStateModel);
        return new StatePersistenceModel(operationStatus, serializedPersistenceModel);
    }

    public bool TryApplyPersistenceModel(StatePersistenceModel persistenceModel)
    {
        if (this.operationStatus != persistenceModel.OperationStatus) return false;
        var storedModelSerializationStrategy = serializationStrategyProvider
            .CreateSerializationStrategy<TPersistedStateModel>();
        TPersistedStateModel deserializedModel = storedModelSerializationStrategy
            .Deserialize(persistenceModel.StateData);
        ApplyPersistedModel(deserializedModel);
        return true;
    }

    protected abstract void ApplyPersistedModel(TPersistedStateModel persistedStateModel);
    protected abstract TPersistedStateModel AccessPersistedModel();
}