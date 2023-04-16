using System.Runtime.CompilerServices;

namespace System
{
    /// <summary>
    /// A set of <see cref="object"/> extension methods
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Create an instance of <see cref="NotImplementedException"/> with message builded based on call context.
        /// </summary>
        /// <param name="target">Current <see cref="object"/>.</param>
        /// <param name="caller">Member source of exception.</param>
        /// <returns>An instance of <see cref="NotImplementedException"/>.</returns>
        public static NotImplementedException GetNotImplementedException(this object target, [CallerMemberName] string? caller = null)
            => new($"{target.GetType().FullName} does not implement {caller}");

        /// <summary>
        /// Create an instance of <see cref="NotSupportedException"/> with message builded based on call context.
        /// </summary>
        /// <param name="target">Current <see cref="object"/>.</param>
        /// <param name="caller">Member source of exception.</param>
        /// <returns>An instance of <see cref="NotSupportedException"/>.</returns>
        public static NotSupportedException GetNotSupportedException(this object target, [CallerMemberName] string? caller = null)
            => new($"{target.GetType().FullName} does not support {caller}");

        /// <summary>
        /// Throw an instance of <see cref="NotImplementedException"/> with message builded based on call context.
        /// Uses <see cref="GetNotImplementedException(object, string?)"/> for build exception instance.
        /// </summary>
        /// <param name="target">Current <see cref="object"/>.</param>
        /// <param name="caller">Member source of exception.</param>
        public static void ThrowNotImplementedException(this object target, [CallerMemberName] string? caller = null)
            => throw target.GetNotImplementedException(caller);

        /// <summary>
        /// Throw an instance of <see cref="NotSupportedException"/> with message builded based on call context.
        /// Uses <see cref="GetNotSupportedException(object, string?)"/> for build exception instance.
        /// </summary>
        /// <param name="target">Current <see cref="object"/>.</param>
        /// <param name="caller">Member source of exception.</param>
        public static void ThrowNotSupportedException(this object target, [CallerMemberName] string? caller = null)
            => throw target.GetNotSupportedException(caller);

        /// <summary>
        /// Retrieves the value of private field.
        /// </summary>
        /// <typeparam name="T">The type of field.</typeparam>
        /// <param name="target">Current <see cref="object"/>.</param>
        /// <param name="fieldName">The name of the field.</param>
        /// <returns>Value of the field.</returns>
        /// <exception cref="MissingFieldException">The field does not exist.</exception>
        public static T? GetNotPublicFieldValue<T>(this object target, string fieldName)
        {
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
#pragma warning restore IDE0079 // Remove unnecessary suppression
            var field = target.GetType().GetField(fieldName, Reflection.BindingFlags.Instance | Reflection.BindingFlags.Static | Reflection.BindingFlags.NonPublic)
                ?? throw new MissingFieldException(target.GetType().FullName, fieldName);
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
#pragma warning restore IDE0079 // Remove unnecessary suppression

            return (T?)field.GetValue(target);
        }

        /// <summary>
        /// Executes an action on target object and returns it.
        /// </summary>
        /// <typeparam name="T">The type of target object.</typeparam>
        /// <param name="target">The target object.</param>
        /// <param name="action">The action to perfonrm on target object.</param>
        /// <returns>The object target of action.</returns>
        public static T ExecuteAndReturn<T>(this T target, Action<T> action)
        {
            action(target);

            return target;
        }

        /// <summary>
        /// Executes an action on target object and returns it.
        /// </summary>
        /// <typeparam name="T">The type of target object.</typeparam>
        /// <param name="target">The target object.</param>
        /// <param name="action">The action to perfonrm on target object.</param>
        /// <returns>The object target of action.</returns>
        public static async Task<T> ExecuteAndReturnAsync<T>(this T target, Func<T, Task> action)
        {
            await action(target);

            return target;
        }

        /// <summary>
        /// Executes an action on target object and returns it.
        /// </summary>
        /// <typeparam name="T">The type of target object.</typeparam>
        /// <param name="target">The target object.</param>
        /// <param name="action">The action to perfonrm on target object.</param>
        /// <param name="cancellation">The cancellation instruction.</param>
        /// <returns>The object target of action.</returns>
        public static async Task<T> ExecuteAndReturnAsync<T>(this T target, Func<T, CancellationToken, Task> action, CancellationToken cancellation)
        {
            await action(target, cancellation);

            return target;
        }
    }
}
