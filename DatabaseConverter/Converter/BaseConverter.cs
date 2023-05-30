using System.Text;

namespace DatabaseConverter
{
    public abstract class BaseConverter
    {
        private string _databaseName;
        private StringBuilder _codeBuilder;

        public BaseConverter(string databaseName) 
        {
            _databaseName = databaseName;
            _codeBuilder = new StringBuilder();

        }

        public abstract bool Convert(string filePath, bool generateConstructor = false);
    }
}
