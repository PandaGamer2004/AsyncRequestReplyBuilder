using System;
using System.Collections.Generic;

namespace AsyncOperationBuilder.Serialization.SerializationProviders.Comparers
{
    public static class IgnoreCaseEqualityComparerSource
    {
        private static readonly Lazy<IEqualityComparer<string>> ComparerInternal = new Lazy<IEqualityComparer<string>>(() 
            => EqualityComparerFactory.Create<string>(EqualsWithIgnoreCase));

        public static IEqualityComparer<string> Comparer 
            => ComparerInternal.Value;
        private static bool EqualsWithIgnoreCase(string first, string second)
            => string.Equals(first, second, StringComparison.InvariantCultureIgnoreCase);
        
    }
}