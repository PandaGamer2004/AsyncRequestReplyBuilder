namespace AsyncOperationBuilder.AsyncOperations.Exceptions;

public class FailedToStartAsyncOperationException: System.Exception
{
    public System.Exception InnerException  { get; private set; }
    
    public FailedToStartAsyncOperationException(string errorMessage, System.Exception innerException)
    :base(errorMessage)
    {
        InnerException = innerException;
    }
}