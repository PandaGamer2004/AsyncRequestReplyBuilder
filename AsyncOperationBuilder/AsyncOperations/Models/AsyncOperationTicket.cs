namespace AsyncOperationBuilder.AsyncOperations.Models;

public class AsyncOperationTicket
{
    public int Id { get; set; }

    public static AsyncOperationTicket NullObject => new()
    {
        Id = -1
    };
}