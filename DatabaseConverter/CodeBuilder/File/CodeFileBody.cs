using DatabaseConverter.CodeBuilder.Attributes;
using System.Text;

namespace DatabaseConverter.CodeBuilder.File
{
    /// <summary>
    /// Represent the content of a class.
    /// </summary>
    public sealed class CodeFileBody : BaseCodeFile
    {
        /// <summary>
        /// Class or interface name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// File is a class (true) or an interface (false).
        /// </summary>
        public bool BodyType { get; set; } = true;

        /// <summary>
        /// Class or interface visibility level.
        /// </summary>
        public VisibilityLevel VisibilityLevel { get; set; } = VisibilityLevel.Public;

        /// <summary>
        /// Body constructors
        /// </summary>
        public List<BodyConstructor> Constructors { get; set; }

        /// <summary>
        /// Class properties.
        /// </summary>
        public List<BodyProperty> Properties { get; set; }

        /// <summary>
        /// Class or interface methods.
        /// </summary>
        public List<BodyMethod> Methods { get; set; }


        public CodeFileBody()
        {
            Properties = new();
            Methods = new();
            Constructors = new();
        }
       
        public override StringBuilder Save(bool lastBracket = false)
        {
            string type = BodyType == true ? "class" : "interface";
          

            var visibilityLevel = VisibilityLevel.ToString().ToLower();

            CodeWriter.AppendLine($"{visibilityLevel} {type} {Name} " + StartBracket);
            
            // Property
            foreach(var property in Properties)
            {
                if (property != null)
                {
                    CodeWriter.AppendLine(property.ToString());
                }
            }


            // Constructors

            foreach(var constructor in Constructors) 
            {
                if (constructor != null)
                {
                    var constructorVisibility = constructor.VisibilityLevel.ToString().ToLower();

                    if (constructor.GetParameters == string.Empty)
                        CodeWriter.AppendLine($"{constructorVisibility} {Name}() {base.StartBracket}");
                    else
                        CodeWriter.AppendLine($"{constructorVisibility} {Name} {constructor.GetParameters} {base.StartBracket}");

                    CodeWriter.AppendLine($"{constructor.Code} {base.EndBracket}");
                }
            }

            // Methods

            foreach(var method in Methods)
            {
                if (method != null)
                {
                    var methodVisibility = method.VisibilityLevel.ToString().ToLower();
                    CodeWriter.AppendLine($"{method.ToString()} {base.StartBracket}");
                    CodeWriter.AppendLine($"{method.Code} {base.EndBracket}");
                }
            }

            CodeWriter.AppendLine(base.EndBracket);

            if (lastBracket) // file has namespace
                CodeWriter.AppendLine(base.EndBracket);
           

            return CodeWriter;
        }
    }

    public sealed class BodyProperty
    {
        public string? Name { get; init; }
        public PropertyType AccessModifier { get; init; }
        public Type? TypeName { get; init; }
        public bool IsArray { get; init; }

        public BodyProperty(string? name, PropertyType accessModifier, Type? typeName, bool isArray)
        {
            Name = name;
            AccessModifier = accessModifier;
            TypeName = typeName;
            IsArray = isArray;
        }

        public override string ToString()
        {
            string propertyType = string.Empty;

            switch (AccessModifier)
            {
                case PropertyType.GetSet:
                    propertyType = "{ get; set; }";
                    break;
                case PropertyType.GetInit:
                    propertyType = "{ get; init; }";
                    break;
                case PropertyType.Get:
                    propertyType = "{ get; }";
                    break;
                case PropertyType.GetPrivateSet:
                    propertyType = "{ get; private set; }";
                    break;
                default:
                    propertyType = "{ get; set; }";
                    break;
            }

            return $"public {Helper.GetTypeName(TypeName!, IsArray)} {Name} {propertyType}";
        }
    }

    public sealed class BodyMethod
    {
        public string? Name { get; init; }
        public Type? MethodType { get; init; }
        public bool IsArray { get; init; }
        public VisibilityLevel? VisibilityLevel { get; init; }
        public List<Variable> Parameters { get; init; } = new();
        public string? Code { get; init; }

        public BodyMethod(string? name, Type? methodType, bool isArray, VisibilityLevel? visibilityLevel, List<Variable> parameters, string? code)
        {
            Name = name;
            MethodType = methodType;
            IsArray = isArray;
            VisibilityLevel = visibilityLevel;
            Parameters = parameters;
            Code = code;
        }

        public override string ToString()
        {
            var visibilityLevel = VisibilityLevel.ToString()?.ToLower();
            string strOut = $"{visibilityLevel} {Helper.GetTypeName(MethodType!, IsArray)} {Name}";

            if (Parameters != null && Parameters.Count > 0 )
                return strOut += GetParameters;
            else
                return string.Concat(strOut, "()");
        }
        private string GetParameters => Parameters != null ? $"({string.Join(',', Parameters)})" : "";
    }

    public sealed class BodyConstructor
    {
        public VisibilityLevel? VisibilityLevel { get; init; }
        public List<Variable> Parameters { get; init; } = new();
        public string? Code { get; init; }

        public BodyConstructor(VisibilityLevel? visibilityLevel, List<Variable> parameters, string? code)
        {
            VisibilityLevel = visibilityLevel;
            Parameters = parameters;
            Code = code;
        }

        public string GetParameters => Parameters != null ? $"({string.Join(',', Parameters)})" : "";
    }

}
