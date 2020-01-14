using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Snail.Common
{
    public class StringHelper
    {
        internal enum SnakeCaseState
        {
            Start,
            Lower,
            Upper,
            NewWord
        }

        public static T ConvertTo<T>(string str)
        {
            if (str == null)
            {
                return default(T);
            }
            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(str);
        }

        /// <summary>
        /// 转小跎峰
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToCamelCase(string s)
        {
            if (string.IsNullOrEmpty(s) || !char.IsUpper(s[0]))
            {
                return s;
            }
            char[] array = s.ToCharArray();
            for (int i = 0; i < array.Length && (i != 1 || char.IsUpper(array[i])); i++)
            {
                bool flag = i + 1 < array.Length;
                if (i > 0 && flag && !char.IsUpper(array[i + 1]))
                {
                    if (char.IsSeparator(array[i + 1]))
                    {
                        array[i] = char.ToLower(array[i], CultureInfo.InvariantCulture);
                    }
                    break;
                }
                array[i] = char.ToLower(array[i], CultureInfo.InvariantCulture);
            }
            return new string(array);
        }

        /// <summary>
        /// 如：AreYouOk转成are_you_ok
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToSnakeCase(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }
            StringBuilder stringBuilder = new StringBuilder();
            SnakeCaseState snakeCaseState = SnakeCaseState.Start;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == ' ')
                {
                    if (snakeCaseState != 0)
                    {
                        snakeCaseState = SnakeCaseState.NewWord;
                    }
                }
                else if (char.IsUpper(s[i]))
                {
                    switch (snakeCaseState)
                    {
                        case SnakeCaseState.Upper:
                            {
                                bool flag = i + 1 < s.Length;
                                if (i > 0 && flag)
                                {
                                    char c = s[i + 1];
                                    if (!char.IsUpper(c) && c != '_')
                                    {
                                        stringBuilder.Append('_');
                                    }
                                }
                                break;
                            }
                        case SnakeCaseState.Lower:
                        case SnakeCaseState.NewWord:
                            stringBuilder.Append('_');
                            break;
                    }
                    char value = char.ToLower(s[i], CultureInfo.InvariantCulture);
                    stringBuilder.Append(value);
                    snakeCaseState = SnakeCaseState.Upper;
                }
                else if (s[i] == '_')
                {
                    stringBuilder.Append('_');
                    snakeCaseState = SnakeCaseState.Start;
                }
                else
                {
                    if (snakeCaseState == SnakeCaseState.NewWord)
                    {
                        stringBuilder.Append('_');
                    }
                    stringBuilder.Append(s[i]);
                    snakeCaseState = SnakeCaseState.Lower;
                }
            }
            return stringBuilder.ToString();
        }
    }
}
