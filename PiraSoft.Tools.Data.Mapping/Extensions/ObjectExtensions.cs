using System.Data;

namespace PiraSoft.Tools.Data.Mapping.Extensions;

public static class ObjectExtensions
{
    public static void Map(this object target, DataRow row)
        => row.Map(target, null);

    public static void Map(this object target, DataRow row, TypeMappings? mappings)
        => Mapper.Map(target, row, mappings);
}
