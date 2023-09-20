
using Newtonsoft.Json;

namespace AsyncOperationBuilder.Serialization.SerializationProviders
{
    public class DataContractHelper
    {
        public T DeserializeJson<T>(string jsonData, JsonSerializerSettings serializerSettings = default)
        {
            var jsonSerializerSettings = serializerSettings ?? new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            };
            return JsonConvert.DeserializeObject<T>(jsonData, jsonSerializerSettings);
        }


        public static string SerializeJson(object data)
        {
            return JsonConvert.SerializeObject(data);
        }
        
        public string SerializeJson(object data, JsonSerializerSettings serializerSettings)
        {
            return JsonConvert.SerializeObject(data, serializerSettings);
        }
        
    }
}
