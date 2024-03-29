﻿namespace System.Data.Common;

/// <summary>
/// A set of <see cref="DbParameterExtensions"/> extension methods.
/// </summary>
public static class DbParameterExtensions
{
    /// <summary>
    /// Returns the string representation of <see cref="DbParameter"/> object.
    /// </summary>
    /// <param name="target">The <see cref="DbParameter"/> object.</param>
    /// <returns>The string representation of <see cref="DbParameter"/> object.</returns>
    public static string? Dump(this DbParameter target)
    {
        if (target.Direction == ParameterDirection.Output)
        {
            return $"{target.ParameterName} = {target.ParameterName} output";
        }
        else if (target.Direction == ParameterDirection.ReturnValue)
        {
            return $"{target.ParameterName} = ";
        }
        else if (target.Value != null)
        {
            return $"{target.ParameterName} = {target.FormatValue()}{(target.Direction == ParameterDirection.InputOutput ? " output" : "")}";
        }
        else
        {
            return null;
        }
    }

    private static string FormatValue(this DbParameter target)
    {
        if (target.Value == DBNull.Value)
        {
            return "NULL";
        }
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        else if (target.Value.GetType().IsEnum)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        {
            return $"{target.Value:D} /*{target.Value}*/";
        }
        else if (target.Value.GetType().IsNumericDatatype())
        {
            return Convert.ToDecimal(target.Value).ToString(Globalization.CultureInfo.InvariantCulture);
        }
        else if (target.Value is DateTime)
        {
            return $"'{target.Value:yyyy-MM-dd HH:mm:ss}'";
        }
        else
        {
            return $"'{target.Value}'";
        }
    }
}