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

        public abstract bool Convert(string filePath, bool generateConstructor = false);
    }
}
