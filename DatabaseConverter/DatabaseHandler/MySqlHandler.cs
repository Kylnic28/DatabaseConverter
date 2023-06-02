using DatabaseConverter.Handler;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System.Net.Sockets;
using System.Reflection;

namespace DatabaseConverter.DatabaseHandler
{
    public class MySqlHandler
    {
        private readonly MySqlConnection _connection;
        public MySqlHandler(MySqlConnectionInfo connectionInfo) 
        {
            _connection = connectionInfo.GetConnection();
        }

        public bool ApplyToDatabase(string mysqlQuery, List<MySqlParameter> parameters, out long lastInsertedId)
        {
            lastInsertedId = 0;

            ArgumentException.ThrowIfNullOrEmpty(mysqlQuery);

            if (parameters == null || parameters.Count < 0)
                throw new ArgumentNullException("Parameters cannot be null or empty!");


            using var command = new MySqlCommand(mysqlQuery, _connection);

            foreach(var parameter in parameters)
                command.Parameters.Add(parameter);

            try {
                this.Open();

                command.Prepare();

                if (command.IsPrepared)
                    command.ExecuteNonQuery();

                lastInsertedId = command.LastInsertedId;

                return true;

            } catch(MySqlException) { return false; }
              finally { this.Close(); }


        }
        public bool ApplyToDatabase(string mysqlQuery, List<MySqlParameter> parameters)
        {
            long lastInsertedId = 0;
            return ApplyToDatabase(mysqlQuery, parameters, out lastInsertedId);
        }
        public Dictionary<string, List<dynamic>> ExecuteQuery(string query, List<MySqlParameter>? parameters = null)
        {
            var dictionary = new Dictionary<string, List<dynamic>>();
            ArgumentException.ThrowIfNullOrEmpty(query);

            try
            {
                this.Open();

                using var command = new MySqlCommand(query, _connection);

                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                        command.Parameters.Add(parameter);
                }

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var columnName = reader.GetName(i);

                        if (!dictionary.ContainsKey(columnName))
                        {
                            dictionary[columnName] = new List<dynamic>();
                        }

                        dictionary[columnName].Add(reader[i]);
                    }
                }
            }
            catch (MySqlException)
            {
                return new Dictionary<string, List<dynamic>>();
            }
            finally { this.Close(); }
            

            return dictionary;
        }

        /// <summary>
        /// Open database connection.
        /// </summary>
        public void Open() => _connection.Open();

        /// <summary>
        /// Close database connection.
        /// </summary>
        public void Close() => _connection.Close();

        /// <summary>
        /// Change database.
        /// </summary>
        /// <param name="newDatabase">Database name.</param>
        public void ChangeDatabase(string newDatabase) => _connection.ChangeDatabase(newDatabase);

        /// <summary>
        /// The SQL instance.
        /// </summary>
        public MySqlConnection Instance => _connection;
    }
}
