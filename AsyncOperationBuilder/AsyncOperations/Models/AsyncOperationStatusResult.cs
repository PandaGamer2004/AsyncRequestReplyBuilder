namespace AsyncOperationBuilder.AsyncOperations.Models;


//Dumb dto without any business logic
//On basic level for integration DTO you have to have only two different states
//Errored with null payload and errorMessage not nullable
//Non errored with nullable payload depends on state and error message nullable
public class AsyncOperationStatusResult<TPayload>
{
    //Invariant
    public bool IsErrored => ErrorMessage != null && Payload == null;
    public string OperationStatus { get; private set; }
    //Can be null
    public TPayload Payload { get; private set; }

    public string ErrorMessage { get; private set; }

    public static AsyncOperationStatusResult<TPayload> CreateErrored(string errorMessage, string operationStatus)
        => new AsyncOperationStatusResult<TPayload>
        {
            ErrorMessage = errorMessage,
            OperationStatus = operationStatus
        };


    public static AsyncOperationStatusResult<TPayload> CreateNotErrored(TPayload payload, string operationStatus)
        => new AsyncOperationStatusResult<TPayload>
        {
            Payload = payload,
            OperationStatus = operationStatus
        };
}