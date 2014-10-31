using System;
using System.Collections.Generic;
using System.Linq;

namespace PoroCYon.Extensions.Collections
{
    /// <summary>
    /// Commonly-used values for the joinWith parameter in <see cref="ext_Collections" />.Join.
    /// </summary>
    /// <remarks>See <see cref="ext_Collections" />.Join for more information.</remarks>
    public static class CommonJoinValues
    {
        /// <summary>
        /// " and "
        /// </summary>
        public readonly static string And = " and ";
        /// <summary>
        /// "->"
        /// </summary>
        public readonly static string Arrow = "->";
        /// <summary>
        /// "\"
        /// </summary>
        public readonly static string Backslash = "\\";
        /// <summary>
        /// ", "
        /// </summary>
        public readonly static string Comma = ", ";
        /// <summary>
        /// "."
        /// </summary>
        public readonly static string Dot = ".";
        /// <summary>
        /// "::"
        /// </summary>
        public readonly static string DoubleColon = "::";
        /// <summary>
        /// " + "
        /// </summary>
        public readonly static string Plus = " + ";
        /// <summary>
        /// "; "
        /// </summary>
        public readonly static string Semicolon = "; ";
        /// <summary>
        /// "/"
        /// </summary>
        public readonly static string Slash = "/";
        /// <summary>
        /// " "
        /// </summary>
        public readonly static string Space = " ";
    }

    /// <summary>
    /// Contains various extension methods for collections.
    /// </summary>
    public static class ext_Collections
    {
        /// <summary>
        /// Extracts a field (or property) from a class instance of each element of <paramref name="collection"/>.
        /// </summary>
        /// <typeparam name="TObject">The element type of <paramref name="collection"/>.</typeparam>
        /// <typeparam name="TField">The type of the elements' field (or property).</typeparam>
        /// <param name="collection">The collection of <typeparamref name="TObject"/>s.</param>
        /// <param name="getField">A function with a <typeparamref name="TObject"/> as parameter and returns a <typeparamref name="TField"/>.</param>
        /// <returns>A collection of <typeparamref name="TField"/>s.</returns>
        public static IEnumerable<TField> Fields<TObject, TField>(this IEnumerable<TObject> collection, Func<TObject, TField> getField)
        {
            List<TField> ret = new List<TField>();

            foreach (TObject obj in collection)
                ret.Add(getField(obj));

            return ret;
        }
        /// <summary>
        /// Casts all elements of <paramref name="collection"/> to <typeparamref name="TNew"/>.
        /// </summary>
        /// <typeparam name="TOld">The type of the elements of <paramref name="collection"/>.</typeparam>.
        /// <typeparam name="TNew">The type of the elements of <paramref name="collection"/> after casting.</typeparam>
        /// <param name="collection">The collection of elements to cast.</param>
        /// <param name="converter">A converter used to cast a <typeparamref name="TOld"/> to a <typeparamref name="TNew"/>.</param>
        /// <returns>A collection of <typeparamref name="TNew"/>s.</returns>
        public static IEnumerable<TNew> CastAll<TOld, TNew>(this IEnumerable<TOld> collection, Converter<TOld, TNew> converter)
        {
            List<TNew> ret = new List<TNew>();

            foreach (TOld obj in collection)
                ret.Add(converter(obj));

            return ret;
        }
        /// <summary>
        /// Joins a collection of <typeparamref name="T" />s with a <see cref="string" />.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="collection" />.</typeparam>
        /// <param name="collection">The collection to join.</param>
        /// <param name="joinWith">The string to join the collection with.</param>
        /// <remarks>If you're using a commonly-used value for <paramref name="joinWith"/>, use a field from <see cref="CommonJoinValues" />.</remarks>
        /// <returns></returns>
        public static string Join<T>(this IEnumerable<T> collection, string joinWith = null)
        {
            return Join(collection, (e, i) => joinWith ?? String.Empty);
        }
        /// <summary>
        /// Joins a collection of <typeparamref name="T" />s with a <see cref="string" />.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="collection" />.</typeparam>
        /// <param name="collection">The collection to join.</param>
        /// <param name="join">The string to join the collection with.</param>
        /// <returns></returns>
        public static string Join<T>(this IEnumerable<T> collection, Func<T, int, string> join = null)
        {
            bool first = true;
            StringBuilder ret = new StringBuilder();

            foreach (T t in collection)
            {
                if (!first && join != null)
                    ret.Append(join);

                ret.Append(t);

                first = false;
            }

            return ret.ToString();
        }
    }
}
