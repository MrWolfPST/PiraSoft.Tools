namespace System.Data.Common;

public static class DbParameterExtensions
{
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
        else if (target.Value.IsNumericDatatype())
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