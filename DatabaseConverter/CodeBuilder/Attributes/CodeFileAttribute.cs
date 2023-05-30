using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConverter.CodeBuilder.Attributes
{
    public enum VisibilityLevel
    {
        None = 0,
        Public = 1,
        Private = 2,
        Protected = 3,
        Internal = 4,
        ProtectedInternal = 5,
        PrivateProtected = 6,
        Abstract = 7,
        Virtual = 8,
    }

    public enum PropertyType
    {
        GetInit,
        Get,
        GetSet,
        GetPrivateSet,
    }

    public class Variable
    {
        public string? Name { get; set; }
        public Type? Type { get; set; }

        private bool _isArray;

        public Variable(string name, Type type, bool isArray = false)
        {
            Name = name;
            Type = type;
            _isArray = isArray;
        }

        public override string ToString()
        {
            // string name;
            return $"{Helper.GetTypeName(Type!, _isArray)} {Name}";
        }
    }
}
