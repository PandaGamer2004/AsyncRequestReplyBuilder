using System;
using System.Collections.Generic;
using AsyncOperationBuilder.Serialization.Interfaces;
using AsyncOperationBuilder.Serialization.SerializationProviders.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AsyncOperationBuilder.Serialization.SerializationProviders.Json.Helpers
{
    public static class JsonProviderFactory
    {
        private static JsonSerializerSettings ignoreCaseSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        public static IDeserializationProvider<string, TResult> CreateIgnoreCaseDeserializationProvider<TResult>()
            => JsonDeserializationProvider<TResult>.Create(ignoreCaseSerializerSettings);
        

        public static ISerializationProvider<TInput, string> CreateIgnoreCaseSerializationProvider<TInput>()
            => JsonSerializationProvider<TInput>.Create(ignoreCaseSerializerSettings);
        
        public static IDeserializationProvider<string, TResult> CreateIgnoreCaseEnumIncludedDeserializationProvider<TResult, TEnum>()
            where TEnum: Enum
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter>
                {
                    JsonEnumConverter.CreateForType<TEnum>()
                },
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            return JsonDeserializationProvider<TResult>.Create(jsonSerializerSettings);
        }

        public static IDeserializationProvider<string, TResult>
            CreateIgnoreCaseEnumIncludedDeserializationProvider<TResult>(IEnumerable<Type> enumTypes)
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter>
                {
                    JsonEnumConverter.CreateForTypes(enumTypes)
                },
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return JsonDeserializationProvider<TResult>.Create(jsonSerializerSettings);
        }
    }
}