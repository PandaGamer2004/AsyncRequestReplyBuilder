using System;
using System.Collections.Generic;
using AsyncOperationBuilder.Serialization.SerializationProviders.Comparers;

namespace AsyncOperationBuilder.Serialization.SerializationProviders.Json.Converters
{
    public class EnumCachedItem
    {
        public EnumCachedItem(Type enumType, IEnumerable<string> enumValues)
        {
            EnumType = enumType;
            CachedEnumValues = new HashSet<string>(enumValues, IgnoreCaseEqualityComparerSource.Comparer);
        }
        public Type EnumType { get; }

        public HashSet<string> CachedEnumValues { get; }
    }
}