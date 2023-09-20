using AsyncOperationBuilder.AsyncOperations.Models;

namespace AsyncOperationBuilder.AsyncOperations.Interfaces;

public interface IPersistedAsyncState{
    public StatePersistenceModel GetPersistenceModel();

    public bool TryApplyPersistenceModel(StatePersistenceModel persistenceModel);
}