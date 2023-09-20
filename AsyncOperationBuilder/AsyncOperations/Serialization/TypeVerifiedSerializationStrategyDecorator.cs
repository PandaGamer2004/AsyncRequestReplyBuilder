using System;
using System.Text;
using AsyncOperationBuilder.AsyncOperations.Cache;
using AsyncOperationBuilder.AsyncOperations.Exceptions;
using AsyncOperationBuilder.AsyncOperations.Helpers;
using AsyncOperationBuilder.AsyncOperations.Interfaces;
using AsyncOperationBuilder.AsyncOperations.Serialization.Interfaces;
using AsyncOperationBuilder.AsyncOperations.Serialization.Models;
using AsyncOperationBuilder.Serialization.SerializationProviders.Json.Helpers;

namespace AsyncOperationBuilder.AsyncOperations.Serialization;

//Decorator design pattern
internal class TypeVerifiedSerializationStrategyDecorator<TStoredAsyncItem>: ISerializationStrategy<TStoredAsyncItem>
{
    private readonly ISerializationStrategy<TStoredAsyncItem> serializationStrategy;


    internal TypeVerifiedSerializationStrategyDecorator(
        ISerializationStrategy<TStoredAsyncItem> serializationStrategy)
    {
        this.serializationStrategy = serializationStrategy;
    }
    private static void VerifyTypeIdentifier(Guid storedIdentifier)
    {
        var typeMetadata = GetTypeMetadata();
        if (typeMetadata.TypeIdentifier != storedIdentifier)
        {
            var errorMessage = new StringBuilder()
                .AppendLine("Failed to deserialize the type from the input stream.")
                .AppendLine($"Type name: {typeof(TStoredAsyncItem)}")
                .AppendLine($"Stored type identifier: {storedIdentifier}")
                .AppendLine($"Expected identifier: {typeMetadata.TypeIdentifier}")
                .ToString();
                
            throw new AsyncStoredItemTypeMismatch(errorMessage, typeof(TStoredAsyncItem));
        }
    }

    private static StoredTypeMetadata GetTypeMetadata()
    {
        var storedItemType = typeof(TStoredAsyncItem);
        StoredTypeMetadata storedTypeMetadata = default(StoredTypeMetadata);
        if (AsyncOperationsStoredTypeResolveCache.HasInCache(storedItemType))
        {
            return AsyncOperationsStoredTypeResolveCache.Get(storedItemType);
        }
        
        var extractedMetadata = MetadataExtractionHelper.ExtractMetadataFrom<TStoredAsyncItem>();
        AsyncOperationsStoredTypeResolveCache.Put(storedItemType, extractedMetadata);
        return extractedMetadata;
    }

    public string Serialize(TStoredAsyncItem storedAsyncItem)
    {
        var serializedContent = serializationStrategy.Serialize(storedAsyncItem);
        var typeMetadata = GetTypeMetadata();
        TypeAwareSerializedModel decoratedModelToSerialize = new TypeAwareSerializedModel
        {
            SerializedContent = serializedContent,
            TypeIdentifier = typeMetadata.TypeIdentifier
        };
        return JsonProviderFactory
            .CreateIgnoreCaseSerializationProvider<TypeAwareSerializedModel>()
            .Serialize(decoratedModelToSerialize);
    }

    public TStoredAsyncItem Deserialize(string inputData)
    {
        var deserializationProvider = JsonProviderFactory
            .CreateIgnoreCaseDeserializationProvider<TypeAwareSerializedModel>();
        TypeAwareSerializedModel typeAwareSerializedModel = deserializationProvider
            .Deserialize(inputData);
        VerifyTypeIdentifier(typeAwareSerializedModel.TypeIdentifier);
        var serializedContent = typeAwareSerializedModel.SerializedContent;
        TStoredAsyncItem storedAsyncItem = this.serializationStrategy.Deserialize(serializedContent);
        return storedAsyncItem;
    }
    
}