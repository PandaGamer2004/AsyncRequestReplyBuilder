using System;
using System.Collections.Generic;
using System.Text;
using AsyncOperationBuilder.AsyncOperations.Serialization.Interfaces;
using AsyncOperationBuilder.Serialization.Interfaces;
using AsyncOperationBuilder.Serialization.SerializationProviders.Json.Helpers;

namespace AsyncOperationBuilder.AsyncOperations.Serialization;

public class SerializersProvider: ISerializationStrategyProvider
{
    private readonly Dictionary<Type, object> typeSerializationCache 
        = new Dictionary<Type, object>();
    
    public ISerializationStrategy<TModel> CreateSerializationStrategy<TModel>()
    {
        ISerializationStrategy<TModel> serializationStrategy = 
            GetSerializationStrategy<TModel>();
        var typeVerifiedDecorator = new TypeVerifiedSerializationStrategyDecorator<TModel>(
            serializationStrategy
            );
        return typeVerifiedDecorator;
    }

    private ISerializationStrategy<TModel> GetSerializationStrategy<TModel>()
    {
        var modelType = typeof(TModel);
        if (typeSerializationCache.TryGetValue(modelType, out var serializer))
        {
            var castedSerializationStrategy = serializer as ISerializationStrategy<TModel>;
            if (castedSerializationStrategy is null)
            {
                var errorMessage = new StringBuilder()
                    .AppendLine($"Failed to get a serializer for the model: {modelType.Name}")
                    .AppendLine("Registered strategy for the type unable to convert to the requested strategy")
                    .AppendLine($"Registered strategy type: {serializer.GetType().Name}")
                    .ToString();
                throw new InvalidOperationException(errorMessage);
            }

            return castedSerializationStrategy;
        }

        var defaultSerializer = CreateDefaultSerializer<TModel>();
        var defaultDeserializer = CreateDefaultDeserializer<TModel>();
        var composedFromProviders =
            new FromSerializationProvidersFactory<TModel>(defaultSerializer, defaultDeserializer);
        this.typeSerializationCache[modelType] = composedFromProviders;
        return composedFromProviders;
    }

    public void RegisterSerializer<TModel>(ISerializationStrategy<TModel> serializationStrategy)
    {
        var modelType = typeof(TModel);
        typeSerializationCache[modelType] = serializationStrategy;
    }

    private ISerializationProvider<TModel, string> CreateDefaultSerializer<TModel>()
        => JsonProviderFactory.CreateIgnoreCaseSerializationProvider<TModel>();


    private IDeserializationProvider<string, TModel> CreateDefaultDeserializer<TModel>()
        => JsonProviderFactory.CreateIgnoreCaseDeserializationProvider<TModel>();
}