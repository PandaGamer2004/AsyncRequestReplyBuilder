namespace Deep.Migrations.Repositories.AsyncOperations
{

    public class AsyncOperationStoreQueryParameters
    {
        public int? TicketId { get; set; }

        public string OperationPayload { get; set; }

        public string OperationStatus { get; set; }
    }
}