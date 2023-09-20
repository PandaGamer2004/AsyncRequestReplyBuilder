using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AsyncOperationBuilder.Serialization.SerializationProviders.Json.Converters
{
    public class JsonEnumConverter : JsonConverter
    {
        private readonly IEnumerable<EnumCachedItem> enumTypeToFields;

        private JsonEnumConverter(IEnumerable<Type> requestedEnumTypes)
        {
            this.enumTypeToFields = CreateEnumTypeToFields(requestedEnumTypes);
        }

        private static IEnumerable<EnumCachedItem> CreateEnumTypeToFields(IEnumerable<Type> requestedEnumTypes)
            => requestedEnumTypes
                .Select(enumType =>
                {
                    var enumValues = Enum.GetNames(enumType);
                    return new EnumCachedItem(enumType, enumValues);
                });

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var targetEnumDescriptor = enumTypeToFields.FirstOrDefault(enumDescriptor => enumDescriptor.EnumType
                == value.GetType());
            if (targetEnumDescriptor is null)
            {
                throw new JsonException($"Unable to match value to serialize: {value} with passed enumTypes");
            }
            string valueName = Enum.GetName(targetEnumDescriptor.EnumType, value);
            writer.WriteValue(valueName);
        }
        

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            bool isTokenStringOrInteger = token.Type == JTokenType.String || token.Type == JTokenType.Integer;
            if (!isTokenStringOrInteger)
            {
                throw new JsonException("Unable to read string from input stream " +
                                               "in converter: {nameof(JsonEnumConverter)}");
            }

            string readValue = token.Value<string>();
            EnumCachedItem targetEnumDescriptor = this.enumTypeToFields.FirstOrDefault(enumTypeDescriptor =>
                                                    enumTypeDescriptor.CachedEnumValues.Contains(readValue));
            if (targetEnumDescriptor is null)
            {
                throw new JsonException($"Failed to match Enum value, with passed. Passed value: {readValue}");
            }

            var parsedEnumValue = Enum.Parse(targetEnumDescriptor.EnumType, readValue, true);
            return parsedEnumValue;
        }

        public override bool CanConvert(Type objectType)
            => this.enumTypeToFields
                .FirstOrDefault(enumDescriptor => enumDescriptor.EnumType == objectType) != null;

        public override bool CanWrite => true;

        public override bool CanRead => true;

        public static JsonEnumConverter CreateForTypes(IEnumerable<Type> enumTypes)
        {
            List<Type> notEnumTypes = enumTypes.Where(enumType =>
                enumType.BaseType != typeof(Enum))
                .ToList();
            
            if (notEnumTypes.Count != 0)
            {
                string errorMessage = "Types that not accepted, because not derived from Enum: " + string.Join(", ", notEnumTypes);
                throw new ArgumentException(errorMessage);
            }

            return new JsonEnumConverter(enumTypes);
        }
        public static JsonEnumConverter CreateForType<TEnumType>()
            where TEnumType : Enum
        {
            var requestedEnumTypes = new[]{typeof(TEnumType)};
            return new JsonEnumConverter(requestedEnumTypes);
        }
    }
}