using AsyncOperationBuilder.Serialization.Interfaces;
using Newtonsoft.Json;

namespace AsyncOperationBuilder.Serialization.SerializationProviders.Json
{
    public class JsonSerializationProvider<TModel>: ISerializationProvider<TModel, string>
    {
        private readonly JsonSerializerSettings settings;
        private readonly DataContractHelper dataContractHelper = new DataContractHelper();

        private JsonSerializationProvider(JsonSerializerSettings settings)
        {
            this.settings = settings;
        }
        public string Serialize(TModel stream)
        {
            string deserializedData = dataContractHelper
                .SerializeJson(stream, settings);
            return deserializedData;
        }

        public static JsonSerializationProvider<TModel> Create(JsonSerializerSettings settings = default)
            => new JsonSerializationProvider<TModel>(settings);
    }
}