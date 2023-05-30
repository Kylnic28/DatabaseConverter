using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConverter.Converter
{
    public class Converter : BaseConverter
    {
        
        public Converter(string databaseName) : base(databaseName)
        {
            Console.WriteLine($"Connect to {databaseName}.");
        }

        public override bool Convert(string filePath, bool generateConstructor = false)
        {
            throw new NotImplementedException();
        }
    }
}
