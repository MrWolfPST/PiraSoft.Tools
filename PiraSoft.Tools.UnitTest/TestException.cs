using System;

namespace PiraSoft.Tools.UnitTest;

internal class TestException : Exception
{
    public TestException() : this(null)
    { }

    public TestException(string? message) : base(message)
    { }
}
