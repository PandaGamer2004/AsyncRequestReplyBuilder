using AsyncOperationBuilder.AsyncOperations.Serialization.Interfaces;
using AsyncOperationBuilder.Serialization.Interfaces;

namespace AsyncOperationBuilder.AsyncOperations.Serialization;

internal class FromSerializationProvidersFactory<TStoredAsyncItem>: ISerializationStrategy<TStoredAsyncItem>
{
    private readonly ISerializationProvider<TStoredAsyncItem, string> serializationProvider;
    private readonly IDeserializationProvider<string, TStoredAsyncItem> deserializationProvider;

    internal FromSerializationProvidersFactory(
        ISerializationProvider<TStoredAsyncItem, string> serializationProvider,
        IDeserializationProvider<string, TStoredAsyncItem> deserializationProvider
        )
    {
        this.serializationProvider = serializationProvider;
        this.deserializationProvider = deserializationProvider;
    }

    public string Serialize(TStoredAsyncItem storedAsyncItem)
        => serializationProvider.Serialize(storedAsyncItem);

    public TStoredAsyncItem Deserialize(string inputData)
        => deserializationProvider.Deserialize(inputData);
}
