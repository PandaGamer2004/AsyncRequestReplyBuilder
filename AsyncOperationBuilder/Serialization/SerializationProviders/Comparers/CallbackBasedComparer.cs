using System;
using System.Collections.Generic;

namespace AsyncOperationBuilder.Serialization.SerializationProviders.Comparers
{
    public class CallbackBasedComparer<TValue>: IEqualityComparer<TValue>
    {
        private readonly Func<TValue, TValue, bool> equals;
        private readonly Func<TValue, int> getHashCode;

        public CallbackBasedComparer(
            Func<TValue, TValue, bool> equals, 
            Func<TValue, int> getHashCode)
        {
            this.equals = equals;
            this.getHashCode = getHashCode;
        }

        public bool Equals(TValue x, TValue y)
            => equals(x, y);

        public int GetHashCode(TValue obj)
            => getHashCode(obj);
    }
}