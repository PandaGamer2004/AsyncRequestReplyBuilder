using AsyncOperationBuilder.Serialization.Interfaces;
using Newtonsoft.Json;

namespace AsyncOperationBuilder.Serialization.SerializationProviders.Json
{
    public class JsonDeserializationProvider<TResult>: IDeserializationProvider<string, TResult>
    {
        private readonly JsonSerializerSettings serializerSettings;
        private readonly DataContractHelper dataContractHelper = new DataContractHelper();

        private JsonDeserializationProvider(JsonSerializerSettings serializerSettings)
        {
            this.serializerSettings = serializerSettings;
        }
        public TResult Deserialize(string inputStream)
        {
            TResult deserializedResult = dataContractHelper
                .DeserializeJson<TResult>(inputStream, serializerSettings);
            return deserializedResult;
        }


        public static JsonDeserializationProvider<TResult> Create(
            JsonSerializerSettings serializerSettings = default)
            => new JsonDeserializationProvider<TResult>(serializerSettings);
    }
}