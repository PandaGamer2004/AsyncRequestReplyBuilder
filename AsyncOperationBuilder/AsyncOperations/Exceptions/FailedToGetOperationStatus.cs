namespace AsyncOperationBuilder.AsyncOperations.Exceptions;

public class FailedToGetOperationStatus: System.Exception
{
    public FailedToGetOperationStatus(string operationError): base(operationError)
    {
        
    }
}