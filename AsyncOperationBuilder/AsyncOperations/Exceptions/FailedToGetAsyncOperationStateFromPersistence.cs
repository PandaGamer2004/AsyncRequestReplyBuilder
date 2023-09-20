namespace AsyncOperationBuilder.AsyncOperations.Exceptions;

public class FailedToGetAsyncOperationStateFromPersistence : System.Exception
{
    public FailedToGetAsyncOperationStateFromPersistence(string message): base(message)
    {
        
    }
}