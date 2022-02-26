using System.Reflection;

namespace PiraSoft.Tools.Core.Extensions
{
    public static class ExceptionExtensions
    {
        public static string? Dump(this Exception exception, int indent = 0)
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
