using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConverter.CodeBuilder
{
    public sealed class Helper
    {
        public static string GetTypeName(Type inputType, bool isArray = false)
        {
            switch (inputType)
            {
                case Type _ when inputType == typeof(char):
                    return isArray ? "char[]" : "char";

                case Type _ when inputType == typeof(string):
                    return isArray ? "string[]" : "string";

                case Type _ when inputType == typeof(long):
                    return isArray ? "long[]" : "long";

                case Type _ when inputType == typeof(ulong):
                    return isArray ? "ulong[]" : "ulong";

                case Type _ when inputType == typeof(int):
                    return isArray ? "int[]" : "int";

                case Type _ when inputType == typeof(uint):
                    return isArray ? "uint[]" : "uint";

                case Type _ when inputType == typeof(short):
                    return isArray ? "short[]" : "short";

                case Type _ when inputType == typeof(ushort):
                    return isArray ? "ushort[]" : "ushort";

                case Type _ when inputType == typeof(sbyte):
                    return isArray ? "sbyte[]" : "sbyte";

                case Type _ when inputType == typeof(byte):
                    return isArray ? "byte[]" : "byte";

                case Type _ when inputType == typeof(float):
                    return isArray ? "float[]" : "float";

                case Type _ when inputType == typeof(double):
                    return isArray ? "double[]" : "double";

                default:
                    return isArray ? string.Concat(inputType.Name, "[]") : inputType.Name;
            }
        }
    }
}
