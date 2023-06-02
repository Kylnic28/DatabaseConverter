using DatabaseConverter.CodeBuilder;
using DatabaseConverter.CodeBuilder.Attributes;
using DatabaseConverter.DatabaseHandler;
using DatabaseConverter.Extensions;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConverter.Converter
{
    public class OracleMySqlConverter : BaseConverter
    {
        private readonly CSharpCodeBuilder _codeBuilder;
        public OracleMySqlConverter(string tableName, MySqlHandler handler) : base(tableName, handler)
        {
            _codeBuilder = new();
        }

        public override bool Convert(string filePath, bool generateConstructor = false, bool generateMethods = false)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));

            Console.WriteLine($"Parsing {_tableName} ...");

            // methods
            StringBuilder updateMethodBuilder = new();
            StringBuilder insertMethodBuilder = new();
            StringBuilder deleteMethodBuilder = new();
            StringBuilder constructorBuilder = new();
            

            var directory = Path.GetDirectoryName(filePath);

            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            _codeBuilder.AppendClass(_tableName.ToPascalCase());

            string query = @$"SELECT * FROM {_tableName} LIMIT 1";
            

            try
            {

                var time = new Stopwatch();
                time.Start();

                _handler.Open();

                using var command = new MySqlCommand(query, _handler.Instance);

                using var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    for(int i = 0; i < reader.FieldCount; i++)
                    {
                        var columnName = reader.GetName(i);
                        var type = reader.GetFieldType(i);

                        _codeBuilder.AppendProperty(columnName.ToPascalCase(), type);

                        if (generateConstructor)
                        {
                            var methodToUse = type == typeof(string) ? $"reader.GetString(\"{columnName}\")" : $"reader.Get{type.Name}(\"{columnName}\")";
                            constructorBuilder.AppendLine($"this.{columnName.ToPascalCase()} = {methodToUse};");
                        }
                    }

                    if (generateMethods)
                    {
                        var columnsNameAsArray = reader.GetColumnSchema().Select(x => x.ColumnName).ToArray();
                        var propertiesNameAsArray = _codeBuilder.Body.Properties.Select(x => string.Concat('@', x.Name)).ToArray();

                        insertMethodBuilder.Append($"return \"INSERT INTO {_tableName} ({string.Join(',', columnsNameAsArray)}) " +
                            $"VALUES ({string.Join(',', propertiesNameAsArray)})\";");

                        updateMethodBuilder.Append($"return \"UPDATE {_tableName} SET ");

                        for(int i = 1; i < columnsNameAsArray.Length; i++)
                            updateMethodBuilder.Append($"{columnsNameAsArray[i]} = {propertiesNameAsArray[i]}, ");

                        updateMethodBuilder.Remove(updateMethodBuilder.Length - 2, 2); // remove last comma (hackfix)

                        updateMethodBuilder.Append($" WHERE {columnsNameAsArray[0]} = {propertiesNameAsArray[0]}\";");
                
                        deleteMethodBuilder.Append($"return \"DELETE FROM {_tableName} WHERE {columnsNameAsArray[0]} = {propertiesNameAsArray[0]};\"");

                        _codeBuilder.AppendMethod("InsertQuery", typeof(string), code: insertMethodBuilder.ToString());
                        _codeBuilder.AppendMethod("UpdateQuery", typeof(string), code: updateMethodBuilder.ToString());
                        _codeBuilder.AppendMethod("DeleteQuery", typeof(string), code: deleteMethodBuilder.ToString());

                    }
                }

                if (generateConstructor)
                {
                    List<Variable> constructorParamaters = new()
                    {
                        new("reader", typeof(MySqlDataReader))
                    };

                    _codeBuilder.AppendConstructor(constructorBuilder.ToString(), constructorParamaters);
                    _codeBuilder.AppendConstructor(); // empty constructor
                }

                _codeBuilder.SaveTo(filePath);
                time.Stop();

                Console.WriteLine($"Done! Source file successfully generated in {time.ElapsedMilliseconds}ms.");
            }
            catch (MySqlException) { return false; }
            finally { _handler.Close(); }



            return true;
        }
    }
}
