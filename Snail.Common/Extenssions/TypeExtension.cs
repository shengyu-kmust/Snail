using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Snail.Common.Extenssions
{
    /// <summary>
    /// The type extensions.
    /// </summary>
    public static class TypeExtension
    {
        /// <summary>
        /// The get dictionary type.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="Type"/>.
        /// </returns>
        public static Type GetDictionaryType(this Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>))
            {
                return type;
            }

            return
                type.GetInterfaces().Where(
                    t =>
                    {
                        if (t.IsGenericType)
                        {
                            return t.GetGenericTypeDefinition() == typeof(IDictionary<,>);
                        }

                        return false;
                    }).FirstOrDefault();
        }

        /// <summary>
        /// The get type of nullable.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="Type"/>.
        /// </returns>
        public static Type GetTypeOfNullable(this Type type)
        {
            return type.GetGenericArguments()[0];
        }

        /// <summary>
        /// The is collection type.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsCollectionType(this Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ICollection<>))
            {
                return true;
            }

            return
                type.GetInterfaces().Where(t => t.IsGenericType).Select(t => t.GetGenericTypeDefinition()).Any(t => t == typeof(ICollection<>));
        }

        /// <summary>
        /// The is dictionary type.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsDictionaryType(this Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>))
            {
                return true;
            }

            return
                type.GetInterfaces().Where(t => t.IsGenericType).Select(t => t.GetGenericTypeDefinition()).Any(t => t == typeof(IDictionary<,>));
        }

        /// <summary>
        /// The is enumerable type.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsEnumerableType(this Type type)
        {
            return type.GetInterfaces().Contains(typeof(IEnumerable));
        }

        /// <summary>
        /// The is list or dictionary type.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsListOrDictionaryType(this Type type)
        {
            if (!type.IsListType())
            {
                return type.IsDictionaryType();
            }

            return true;
        }

        /// <summary>
        /// The is list type.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsListType(this Type type)
        {
            return type.GetInterfaces().Contains(typeof(IList));
        }

        /// <summary>
        /// The is nullable type.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsNullableType(this Type type)
        {
            if (type.IsGenericType)
            {
                return type.GetGenericTypeDefinition() == typeof(Nullable<>);
            }

            return false;
        }
    }

}
