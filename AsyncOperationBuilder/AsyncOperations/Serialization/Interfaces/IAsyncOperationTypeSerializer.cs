namespace AsyncOperationBuilder.AsyncOperations.Serialization.Interfaces;

public interface IAsyncOperationTypeSerializer
{
    public ISerializationStrategy<TPayload> Create<TPayload>();
}