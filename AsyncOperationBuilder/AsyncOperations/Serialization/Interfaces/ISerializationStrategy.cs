namespace AsyncOperationBuilder.AsyncOperations.Serialization.Interfaces;

public interface ISerializationStrategy<TStoredAsyncItem>
{
    public string Serialize(TStoredAsyncItem storedAsyncItem);

    public TStoredAsyncItem Deserialize(string inputData);
}