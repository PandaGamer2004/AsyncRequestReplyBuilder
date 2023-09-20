namespace AsyncOperationBuilder.AsyncOperations.Interfaces.State;

public interface IAsyncStateRoot<TPayload>
{
    public void SetState(IAsyncOperationAsyncState<TPayload> operationAsyncState);
}