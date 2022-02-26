using System.Reflection;

namespace PiraSoft.Tools.Core.Extensions
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Create a detailed dump of exception data
        /// </summary>
        /// <param name="target"></param>
        /// <param name="indent"></param>
        /// <returns></returns>
        public static string? Dump(this Exception target)
            => target.Dump(0);

        private static string? Dump(this Exception exception, int indent)
        {
            if (exception == null)
                return null;

            try
            {
                var properties = exception.GetType().GetTypeInfo().DeclaredProperties;
                var fields = from p in properties select $"{new string('\t', indent)}{p.Name} = {p.GetValue(exception, null) ?? string.Empty}";

                return $"{exception.GetType().FullName}\r\n{string.Join("\r\n", fields)}\r\n{exception?.InnerException?.Dump(indent++)}";
            }
            catch
            {
                return exception.ToString();
            }
        }
    }
}
