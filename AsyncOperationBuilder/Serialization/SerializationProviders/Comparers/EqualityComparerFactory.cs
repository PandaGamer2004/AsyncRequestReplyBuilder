using System;
using System.Collections.Generic;

namespace AsyncOperationBuilder.Serialization.SerializationProviders.Comparers
{
    public static class EqualityComparerFactory
    {
        public static IEqualityComparer<TResult> Create<TResult>(Func<TResult, TResult, bool> equals,
            Func<TResult, int> getHashCode)
            => new CallbackBasedComparer<TResult>(equals, getHashCode);

        public static IEqualityComparer<TResult> Create<TResult>(Func<TResult, TResult, bool> equals)
            => new CallbackBasedComparer<TResult>(equals, GetHashCodeByDefault);

        private static int GetHashCodeByDefault<TInput>(TInput inputObject)
            => EqualityComparer<TInput>.Default.GetHashCode(inputObject);
        
    }
}