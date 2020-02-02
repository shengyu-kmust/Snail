using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snail.Common
{
    /// <summary>
    /// 用于替代，lock (string.Intern("xx"))}参考https://stackoverflow.com/questions/6983714/locking-on-an-interned-string
    /// </summary>
    public class Locker
    {
        private static ConcurrentDictionary<string,LockerEntry> lockers=new ConcurrentDictionary<string, LockerEntry>();
        private static DateTime _lastExpirationScan;//最后一次
        private static TimeSpan ExpirationScanFrequency=new TimeSpan(0,1,0);//默认5分钟清理一次锁内存

        /// <summary>
        /// 获取锁，此锁会在1分钟后从内存里移除。避免过多造成内存过大
        /// </summary>
        /// <param name="key">key值</param>
        /// <returns></returns>
        public static object GetLocker(string key)
        {
            return GetLocker(key,new TimeSpan(0,1,0));
        }
        /// <summary>
        /// 获取锁，并设置此锁多久后从内存里移除。避免过多造成内存过大
        /// </summary>
        /// <param name="key">key值</param>
        /// <param name="expiration">多久过期</param>
        /// <returns></returns>
        public static object GetLocker(string key,TimeSpan expiration)
        {
            StartScanForExpiredItems();
            return lockers.GetOrAdd(key, new LockerEntry { _absoluteExpiration=DateTime.Now.Add(expiration)});
        }

        /// <summary>
        /// 将所有锁从内存里清除
        /// </summary>
        public static void ClearLockers()
        {
            lockers.Clear();
        }

        private static void StartScanForExpiredItems()
        {
            var now = DateTime.Now;
            if (ExpirationScanFrequency < now - _lastExpirationScan)
            {
                _lastExpirationScan = now;
                Task.Run(() =>
                {
                    foreach (var item in lockers)
                    {
                        if (item.Value._absoluteExpiration<now)
                        {
                            lockers.TryRemove(item.Key,out LockerEntry value);
                        }
                    }
                });
            }
        }

        internal class LockerEntry
        {
            internal DateTime _absoluteExpiration;//什么时候释放
        }
    }
}
