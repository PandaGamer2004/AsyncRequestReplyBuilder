using System;
using System.Collections.Generic;
using System.Text;
using AsyncOperationBuilder.AsyncOperations.Exceptions;
using AsyncOperationBuilder.AsyncOperations.Interfaces;

namespace AsyncOperationBuilder.AsyncOperations.Cache;

public class AsyncOperationsStoredTypeResolveCache
{
    private static readonly Dictionary<Type, StoredTypeMetadata> Cache = new Dictionary<Type, StoredTypeMetadata>();

    public static void Put(Type key, StoredTypeMetadata metadataToStore)
    {
        if (Cache.TryGetValue(key, out var storedTypeMetadata))
        {           
            if (!storedTypeMetadata.Equals(metadataToStore))
            {
                var errorString = new StringBuilder()
                    .AppendLine($"Failed to store to the cache item with key: {key}.")
                    .AppendLine($"Already exists value: {storedTypeMetadata}")
                    .AppendLine($"Than not equal to passed: ${metadataToStore}")
                    .ToString();
                throw new CacheException(errorString);
            }
        }
        Cache[key] = metadataToStore;
    }

    public static StoredTypeMetadata Get(Type key)
        => Cache.TryGetValue(key, out var storedValue) ? storedValue : null;

    public static bool HasInCache(Type key)
        => Get(key) != null;

}