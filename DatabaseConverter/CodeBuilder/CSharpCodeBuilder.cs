using DatabaseConverter.CodeBuilder.Attributes;
using DatabaseConverter.CodeBuilder.File;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;


namespace DatabaseConverter.CodeBuilder
{
    /// <summary>
    /// Generate C# code.
    /// </summary>
    public sealed class CSharpCodeBuilder : ICodeBuilder
    {
        private CodeFileHeader _header;
        private CodeFileBody _body;
        public CodeFileHeader Header { get => _header; }
        public CodeFileBody Body { get => _body; }

        private bool _hasNamespace;
        public CSharpCodeBuilder()
        {
            _header = new CodeFileHeader();
            _body = new CodeFileBody();

        }
        public void AppendClass(string className, VisibilityLevel accessModifier = VisibilityLevel.Public)
        {
            _body.Name = className;
            _body.BodyType = true;
            _body.VisibilityLevel = accessModifier;
        }

        public void AppendConstructor(string code = "", List<Variable>? parameters = null, VisibilityLevel visibilityLevel = VisibilityLevel.Public) => _body.Constructors.Add(new(visibilityLevel, parameters, code));

        public void AppendInterface(string interfaceName, VisibilityLevel accessModifier = VisibilityLevel.Public)
        {
            _body.Name = interfaceName;
            _body.BodyType = false;
            _body.VisibilityLevel = accessModifier;
        }

        public void AppendMethod(string methodName, Type type, bool isArray = false, List<Variable>? parameters = null, string code = "", VisibilityLevel accessModifier = VisibilityLevel.Public) => _body.Methods.Add(new(methodName, type, isArray, accessModifier, parameters!, code));
        public void AppendNamespace(string namespaceName)
        {
            _header.Namespace = namespaceName;
            _hasNamespace = true;
        }
        public void AppendUsing(string usingName) => _header.Usings.Add(usingName);
        public void AppendProperty(string propertyName, Type propertyType, bool isArray = false, PropertyType propertyAccessibility = PropertyType.GetSet) => _body.Properties.Add(new(propertyName, propertyAccessibility, propertyType, isArray));


        public void SaveTo(string sourceFile)
        {
            using var writer = new StreamWriter(sourceFile);

            string code = _header.Save()
                                  .Append(_body.Save(_hasNamespace)).ToString();

            writer.Write(FormatCode(code));
        }

        private string FormatCode(string src) 
        {
            return CSharpSyntaxTree.ParseText(src).GetRoot().NormalizeWhitespace().SyntaxTree.GetText().ToString();
        }


    }
}
