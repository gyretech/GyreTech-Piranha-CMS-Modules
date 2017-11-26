namespace System 
{
    internal static class Throw
    {
        #region NullOrEmpty

        private const string COMMON_NULL_OR_EMPTY = "Value cannot be null or empty.";

        public static string IfArgumentNullOrEmpty(string arg, string argName)
        {
            return IfArgumentNullOrEmpty(arg, argName, null /* message */);
        }

        public static string IfArgumentNullOrEmpty(string arg, string argName, string message)
        {
            if (string.IsNullOrEmpty(arg)) throw new ArgumentException(message ?? COMMON_NULL_OR_EMPTY, argName);
            return arg;
        }

        #endregion

        #region Null

        private const string COMMON_NULL = "Value cannot be null";

        public static T IfArgumentNull<T>(T arg, string argName) where T : class
        {
            return IfArgumentNull(arg, argName, null /* message */);
        }

        public static T IfArgumentNull<T>(T arg, string argName, string message) where T : class
        {
            if (null == arg) throw new ArgumentNullException(argName, message ?? COMMON_NULL);
            return arg;
        }

        #endregion

        #region If

        private const string COMMON_IF = "Value is not valid";

        public static T IfArgument<T>(T arg, Func<T, bool> condition, string argName)
        {
            return IfArgument(arg, condition, argName, null /* message */);
        }

        public static T IfArgument<T>(T arg, Func<T, bool> condition, string argName, string message)
        {
            if (condition.Invoke(arg)) throw new ArgumentException(message ?? COMMON_NULL, argName);
            return arg;
        }

        #endregion

    }
}
