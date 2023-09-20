using System.Reflection;
using AsyncOperationBuilder.AsyncOperations.Attributes;
using AsyncOperationBuilder.AsyncOperations.Exceptions;
using AsyncOperationBuilder.AsyncOperations.Interfaces;

namespace AsyncOperationBuilder.AsyncOperations.Helpers;

public static class MetadataExtractionHelper
{
    public static StoredTypeMetadata ExtractMetadataFrom<TInput>()
    {
        AsyncOperationPersistedTypeIdentifier persistedTypeIdentifier = 
            typeof(TInput).GetCustomAttribute<AsyncOperationPersistedTypeIdentifier>();
        if (persistedTypeIdentifier == null)
        {
            throw new LackOfMetadataException(
                $"Failed fetch on type: {typeof(TInput).Name} attribute: {nameof(AsyncOperationPersistedTypeIdentifier)}", 
                typeof(TInput));
        }

        var typeIdentifier = persistedTypeIdentifier.TypeIdentifier;

        return new StoredTypeMetadata
        {
            TypeIdentifier = typeIdentifier,
        };
    }
}