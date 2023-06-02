using DatabaseConverter.DatabaseHandler;
using System.Text;

namespace DatabaseConverter
{
    public abstract class BaseConverter
    {
        protected readonly string _tableName;

        protected readonly MySqlHandler _handler;
        public BaseConverter(string tableName, MySqlHandler handler) 
        {
            _tableName = tableName;
            _handler = handler;
        }

        /// <summary>
        /// Generate source file.
        /// </summary>
        /// <param name="filePath">Path to source file.</param>
        /// <param name="generateConstructor">Constructor will be generated.</param>
        /// <param name="generateMethods">Methods will be generated.</param>
        /// <returns>True if success, otherwise, false.</returns>
        public abstract bool Convert(string filePath, bool generateConstructor = false, bool generateMethods = false);
    }
}
