using PiraSoft.Tools.Data.Mapping;
using System.Data;

namespace System;

public static class ObjectExtensions
{
    public static void Map(this object target, DataRow row)
        => target.Map(row, null);

    public static void Map(this object target, DataRow row, TypeMappings? mappings)
        => row.Map(target ?? throw new ArgumentNullException(nameof(target)), mappings);
}
