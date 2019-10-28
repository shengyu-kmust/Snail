using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Snail.Common
{
    /// <summary>
    /// 用于替代，lock (string.Intern("xx"))}参考https://stackoverflow.com/questions/6983714/locking-on-an-interned-string
    /// </summary>
    public class Locker
    {
        private static ConcurrentDictionary<string,object> lockers=new ConcurrentDictionary<string, object>();
        
        public static object GetLocker(string key)
        {
            
            return lockers.GetOrAdd(key, new object());
        }

        public static void ClearLockers()
        {
            lockers.Clear();
        }
    }
}
