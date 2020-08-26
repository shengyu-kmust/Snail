using System;
using System.Collections.Generic;

namespace Snail.Common
{
    /// <summary>
    /// IEqualityComparer的通用实现类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericEqualityCompare<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _compareFunc;
        private readonly Func<T, int> _hashFunc;
        public GenericEqualityCompare(Func<T, T, bool> compareFunc, Func<T, int> hashFunc)
        {
            this._compareFunc = compareFunc;
            this._hashFunc = hashFunc;
        }
        public bool Equals(T x, T y)
        {
            return _compareFunc(x, y);

        }

        public int GetHashCode(T obj)
        {
            return _hashFunc(obj);
        }
    }
}
