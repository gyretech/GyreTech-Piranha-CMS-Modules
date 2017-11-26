using System.Collections.Generic;

namespace System
{
    internal static class SystemExtensions
    {
        public static void Each<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));

            if (action == null) return;

            foreach (var item in enumerable)
            {
                action(item);
            }
        }

        /// <summary>Checks whatever given collection object is null or has no item.</summary>
        public static bool IsNullOrEmpty<T>(this ICollection<T> source)
        {
            return source == null || source.Count <= 0;
        }

        /// <summary>Adds an item to the collection if it's not already in the collection.</summary>
        /// <param name="source">Collection</param>
        /// <param name="item">Item to check and add</param>
        /// <typeparam name="T">Type of the items in the collection</typeparam>
        /// <returns>Returns True if added, returns False if not.</returns>
        public static bool AddIfNotContains<T>(this ICollection<T> source, T item)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Contains(item)) return false;

            source.Add(item);

            return true;
        }

        /// <summary>This method is used to try to get a value in a dictionary if it does exists.</summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="dictionary">The collection object</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value of the key (or default value if key not exists)</param>
        /// <returns>True if key does exists in the dictionary</returns>
        public static bool TryGetValueOrDefault<T>(this IDictionary<string, object> dictionary, string key, out T value)
        {
            object valueObj;
            if (dictionary.TryGetValue(key, out valueObj) && valueObj is T)
            {
                value = (T)valueObj;
                return true;
            }

            value = default(T);
            return false;
        }

        /// <summary>Gets a value from the dictionary with given key. Returns default value if can not find.</summary>
        /// <param name="dictionary">Dictionary to check and get</param>
        /// <param name="key">Key to find the value</param>
        /// <typeparam name="TKey">Type of the key</typeparam>
        /// <typeparam name="TValue">Type of the value</typeparam>
        /// <returns>Value if found, default if can not found.</returns>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : default(TValue);
        }

        public static string InnerMostMessage(this Exception exception)
        {
            var ex = InnerMostException(exception);
            return ex == null ? string.Empty : ex.Message;
        }

        public static Exception InnerMostException(this Exception exception)
        {
            Throw.IfArgumentNull(exception, "exception");

            Exception r = exception;
            while (r.InnerException != null) { r = r.InnerException; }

            return r;
        }

        public static System.Security.SecureString ToSecureString(this string source, bool makeReadonly = true)
        {
            if (string.IsNullOrEmpty(source)) return null;

            var r = new System.Security.SecureString();
            foreach (char c in source)
            {
                r.AppendChar(c);
            }

            if (makeReadonly) r.MakeReadOnly();

            return r;
        }

        public static string ToUnsecureString(this System.Security.SecureString secureString)
        {
            if (secureString == null) throw new ArgumentNullException(nameof(secureString));

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return System.Runtime.InteropServices.Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }


        public static bool EqualsOrdinalIgnoreCase(this string value, string other)
        {
            return string.Equals(value, other, StringComparison.OrdinalIgnoreCase);
        }

        public static bool EqualsOrdinal(this string value, string other)
        {
            return string.Equals(value, other, StringComparison.Ordinal);
        }

        public static bool EqualsInvariantCultureIgnoreCase(this string value, string other)
        {
            return string.Equals(value, other, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool EqualsInvariantCulture(this string value, string other)
        {
            return string.Equals(value, other, StringComparison.InvariantCulture);
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }



    }
}
