using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
namespace Snail.Common.Extenssions
{
    public static class EnumExtenssion
    {
        public static string GetDisplayName<TEnum>(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            try
            {
                var model = Parse<TEnum>(value);
                return (model as Enum).GetDisplayName();
            }
            catch (Exception)
            {
                return value;
            }

        }

        public static string GetDisplayName(this Enum enumModel)
        {
            var enumType = enumModel.GetType();

            // 获取枚举常数名称。
            var name = Enum.GetName(enumType, enumModel);
            if (string.IsNullOrEmpty(name)) return null;

            // 获取枚举字段。
            var fieldInfo = enumType.GetField(name);
            if (fieldInfo != null)
            {
                // 获取描述的属性。
                if (Attribute.GetCustomAttribute(fieldInfo, typeof(DisplayAttribute), false) is DisplayAttribute attr)
                {
                    return attr.GetName();
                }

                return fieldInfo.Name;
            }
            return null;
        }

        /// <summary>
        /// Arrays the is valiad.
        /// </summary>
        /// <param name="array">
        /// The array.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool ArrayIsValid(this Array array)
        {
            if (array != null && array.Length > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// The get description.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <typeparam name="TEnum">the TEnum</typeparam>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetDescription<TEnum>(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            try
            {
                var model = Parse<TEnum>(value);
                return (model as Enum).GetDescription();
            }
            catch (Exception)
            {
                return value;
            }
        }

        /// <summary>
        /// The get description.
        /// </summary>
        /// <param name="enumModel">
        /// The enum model.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetDescription(this Enum enumModel)
        {
            var enumType = enumModel.GetType();

            // 获取枚举常数名称。
            string name = Enum.GetName(enumType, enumModel);
            if (!string.IsNullOrEmpty(name))
            {
                // 获取枚举字段。
                var fieldInfo = enumType.GetField(name);
                if (fieldInfo != null)
                {
                    // 获取描述的属性。
                    if (Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute), false) is DescriptionAttribute attr)
                    {
                        return attr.Description;
                    }
                }
            }

            return null;
        }


        /// <summary>
        /// The get enum values.
        /// </summary>
        /// <typeparam name="TEnum">the TEnum</typeparam>
        /// <returns>
        /// The <see cref="IList"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// throw ArgumentException
        /// </exception>
        public static IList<TEnum> GetEnumValues<TEnum>()
        {
            var type = typeof(TEnum);
            if (!type.IsEnum)
            {
                throw new ArgumentException("Type is not enum.");
            }

            return
                (from field in type.GetFields(BindingFlags.Public | BindingFlags.Static)
                 select (TEnum)field.GetValue(type)).ToArray();
        }

        /// <summary>
        /// 获取枚举名称
        /// </summary>
        /// <param name="enumModel">the enumModel</param>
        /// <returns>return string</returns>
        public static string GetName(this Enum enumModel)
        {
            var enumType = enumModel.GetType();
            return Enum.GetName(enumType, enumModel);
        }



        /// <summary>
        /// The parse.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <typeparam name="TEnum">the TEnum</typeparam>
        /// <returns>
        /// The <see cref="TEnum"/>.
        /// </returns>
        public static TEnum Parse<TEnum>(string value)
        {
            try
            {
                var enumType = typeof(TEnum);
                if (enumType.IsNullableType())
                {
                    enumType = enumType.GetTypeOfNullable();
                }

                return (TEnum)Enum.Parse(enumType, value);
            }
            catch
            {
                return default(TEnum);
            }
        }

        /// <summary>
        /// To the hexadecimal string.
        /// </summary>
        /// <param name="hex">
        /// The hexadecimal.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToHexString(this byte[] hex)
        {
            if (hex == null)
            {
                return null;
            }

            if (hex.Length == 0)
            {
                return string.Empty;
            }

            var s = new StringBuilder();
            foreach (var b in hex)
            {
                s.Append(b.ToString("x2"));
            }

            return s.ToString();
        }
    }
}
