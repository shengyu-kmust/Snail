using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
namespace Snail.Common
{
    public static class HashHelper
    {
        public static string Md5(string content)
        {
            return HashCompute(HashAlgorithmName.MD5,content);
        }

        /// <summary>
        /// 计算哈希值，并返回16进制表示的字符串
        /// </summary>
        /// <param name="hashAlgorithmName">哈希算法名称</param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string HashCompute(HashAlgorithmName hashAlgorithmName,string content)
        {
            
            return BitConverter.ToString(HashAlgorithm.Create(hashAlgorithmName.Name).ComputeHash(Encoding.UTF8.GetBytes(content))).Replace("-", "");
        }

        /// <summary>
        /// 计算基于密钥的哈希值，并返回166进制表示的字符串
        /// </summary>
        /// <param name="content"></param>
        /// <param name="key"></param>
        /// <param name="algorithmName"></param>
        /// <returns></returns>
        public static string HMACCompute(string content,string key,string algorithmName)
        {
            var hmac = HMAC.Create(algorithmName);
            hmac.Key = Encoding.UTF8.GetBytes(key);
            return BitConverter.ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(content))).Replace("-", "");
        }
    }
}
