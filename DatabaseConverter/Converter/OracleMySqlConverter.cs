using DatabaseConverter.CodeBuilder;
using DatabaseConverter.CodeBuilder.Attributes;
using DatabaseConverter.DatabaseHandler;
using DatabaseConverter.Extensions;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConverter.Converter
{
    public class OracleMySqlConverter : BaseConverter
    {
        private readonly CSharpCodeBuilder _codeBuilder;
        private readonly StringBuilder _stringBuilder;
        public OracleMySqlConverter(string tableName, MySqlHandler handler) : base(tableName, handler)
        {
            _codeBuilder = new();
            _stringBuilder = new();
            Console.WriteLine($"Parsing {tableName} ...");
        }

        public override bool Convert(string filePath, bool generateConstructor = false)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));

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

                        if (generateConstructor)
                        {
                            var methodToUse = type == typeof(string) ? $"reader.GetString(\"{columnName}\")" : $"reader.Get{type.Name}(\"{columnName}\")";
                            _stringBuilder.AppendLine($"this.{columnName.ToPascalCase()} = {methodToUse};");
                        }

                        _codeBuilder.AppendProperty(columnName.ToPascalCase(), type);
                    }
                }

                if (generateConstructor)
                {
                    List<Variable> constructorParamaters = new()
                    {
                        new("reader", typeof(MySqlDataReader))
                    };

                    _codeBuilder.AppendConstructor(_stringBuilder.ToString(), constructorParamaters);
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
