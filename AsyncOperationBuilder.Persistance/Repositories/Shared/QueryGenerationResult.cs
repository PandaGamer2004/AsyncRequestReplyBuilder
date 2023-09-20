namespace AsyncOperationBuilder.Persistance.Repositories.Shared
{
    public class QueryGenerationResult<TParameters>
    {
        public string Query { get; set; }

        public TParameters Parameters { get; set; }
    }
}