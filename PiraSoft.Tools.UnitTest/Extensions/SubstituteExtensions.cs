using NSubstitute.Core;
using System;
using System.Threading.Tasks;

namespace NSubstitute;

internal static class SubstituteExtensions
{
    public static ConfiguredCall Throw<T>(this T value, Exception exception)
    {
        if (exception == null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        return value.Returns(x => throw exception);
    }

    public static ConfiguredCall ThrowAsync<T>(this Task<T> value, Exception exception)
    {
        if (exception == null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        return value.Returns(x => Task.FromException<T>(exception));
    }}
