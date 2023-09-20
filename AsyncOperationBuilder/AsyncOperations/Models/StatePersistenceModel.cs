namespace AsyncOperationBuilder.AsyncOperations.Models;

public class StatePersistenceModel
{
    //State will be selected by that rule
    public AsyncOperationStatus OperationStatus { get; }
    
    //Here we will encapsulate all data needed to restore state
    public string StateData { get;  }

    public StatePersistenceModel(
        AsyncOperationStatus operationStatus,
        string stateData)
    {
        OperationStatus = operationStatus;
        StateData = stateData;
    }
}
