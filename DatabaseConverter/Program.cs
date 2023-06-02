// See https://aka.ms/new-console-template for more information

using DatabaseConverter.CodeBuilder;
using DatabaseConverter.CodeBuilder.Attributes;
using DatabaseConverter.Converter;
using DatabaseConverter.DatabaseHandler;
using DatabaseConverter.Extensions;

MySqlHandler handler = new(new("127.0.0.1", 3306, "Helnesis", "helnesis", "world_classic"));

var table = handler.ExecuteQuery("SHOW TABLES");

foreach(var data in table)
{
    foreach(var value in data.Value)
    {
        var tableName = value as string;

        if (tableName != null)
        {
            OracleMySqlConverter converter = new(tableName, handler);
            converter.Convert($"sourcefiles/{tableName.ToPascalCase()}.cs", true, true);
        }

    }
}

