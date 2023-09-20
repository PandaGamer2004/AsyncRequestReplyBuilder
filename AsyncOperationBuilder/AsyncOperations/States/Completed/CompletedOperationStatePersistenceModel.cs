namespace AsyncOperationBuilder.AsyncOperations.States.Completed;

public class CompletedOperationStatePersistenceModel<TResult>
{
    public TResult OperationResult { get; set; }
}