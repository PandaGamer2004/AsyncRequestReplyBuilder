namespace AsyncOperationBuilder.AsyncOperations.Serialization.Interfaces;

public interface ISerializationStrategyProvider
{
    public ISerializationStrategy<TModel> CreateSerializationStrategy<TModel>();
}