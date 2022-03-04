namespace System.Data.Common;

public static class DbParameterExtensions
{
    public static string? Dump(this DbParameter target)
    {
        if (target.Value != null)
        {
            return $"{target.ParameterName} = {target.FormatValue()}";
        }
        else if (target.IsOutput())
        {
            return $"{target.ParameterName} = {target.ParameterName} output";
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
        else if (target.GetType().IsEnum)
        {
#pragma warning disable CS8605 // Unboxing a possibly null value.
            return $"{(long)target.Value} /*{target.Value}*/";
        }
#pragma warning restore CS8605 // Unboxing a possibly null value.
#pragma warning disable CS8604 // Possible null reference argument.
        else if (target.Value.IsNumericDatatype())
        {
#pragma warning restore CS8604 // Possible null reference argument.
            return Convert.ToDecimal(target.Value).ToString(Globalization.CultureInfo.InvariantCulture);
        }
        else
        {
            return $"'{target.Value}'";
        }
    }

    private static bool IsOutput(this DbParameter target)
        => target.Direction == ParameterDirection.InputOutput || target.Direction == ParameterDirection.Output;
}