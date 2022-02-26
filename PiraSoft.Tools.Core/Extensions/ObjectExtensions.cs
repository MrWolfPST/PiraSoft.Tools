using System.Runtime.CompilerServices;

namespace System
{
    public static class ObjectExtensions
    {
        public static NotImplementedException GetNotImplementedException(this object target, [CallerMemberName] string? caller = null)
        {
            return new NotImplementedException($"{target.GetType().FullName} does not implement {caller}");
        }

        public static NotSupportedException GetNotSupportedException(this object target, [CallerMemberName] string? caller = null)
        {
            return new NotSupportedException($"{target.GetType().FullName} does not support {caller}");
        }

        public static void ThrowNotImplementedException(this object target, [CallerMemberName] string? caller = null)
        {
            throw target.GetNotImplementedException(caller);
        }

        public static void ThrowNotSupportedException(this object target, [CallerMemberName] string? caller = null)
        {
            throw target.GetNotSupportedException(caller);
        }
    }
}
