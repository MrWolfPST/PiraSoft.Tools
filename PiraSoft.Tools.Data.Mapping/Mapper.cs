using System.Data;

namespace PiraSoft.Tools.Data.Mapping;

//TODO: Implement internal type mapping cache
//TODO: Implement public type mapping catalog

public static class Mapper
{
    public static object Map(Func<object> factory, DataRow row)
        => Map(factory, row, new Dictionary<string, MappingBase>());

    public static object Map(Func<object> factory, DataRow row, Dictionary<string, MappingBase>? mappings)
    {
        if (factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        var retVal = factory();

        if (retVal == null)
        {
            throw new InvalidOperationException($"{nameof(factory)} returns a null object.");
        }

        Map(retVal, row, mappings);

        return retVal;
    }

    public static void Map(object target, DataRow row)
        => Map(target, row, new Dictionary<string, MappingBase>());

    public static void Map(object target, DataRow row, Dictionary<string, MappingBase>? mappings)
    {
        if (target == null)
        {
            throw new ArgumentNullException(nameof(target));
        }

        if (row == null)
        {
            throw new ArgumentNullException(nameof(row));
        }

        GetMappings(target.GetType(), mappings).ForEach(i => i.Map(target, row));
    }

    private static PropertyInfoMappingCollection GetMappings(Type type, Dictionary<string, MappingBase>? mappings = null, string fieldNamePattern = "{0}")
    {
        var retVal = new PropertyInfoMappingCollection();

        type.GetProperties().Where(i => i.CanWrite).ForEach(i => retVal.AddMapping(i, mappings, fieldNamePattern));

        return retVal;
    }
}
