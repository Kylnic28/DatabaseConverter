using DatabaseConverter.CodeBuilder.Attributes;


namespace DatabaseConverter.CodeBuilder
{
    public interface ICodeBuilder
    {
        /// <summary>
        /// Add a class.
        /// </summary>
        /// <param name="className">Class name.</param>
        void AppendClass(string className, VisibilityLevel accessModifier = VisibilityLevel.Public);

        /// <summary>
        /// Add an interface.
        /// </summary>
        /// <param name="interfaceName">Interface name.</param>
        void AppendInterface(string interfaceName, VisibilityLevel accessModifier = VisibilityLevel.Public);

        /// <summary>
        /// Add a method.
        /// </summary>
        /// <param name="method">Method name.</param>
        void AppendMethod(string methodName, Type type, bool isArray = false, List<Variable>? parameters = null, string code = "", VisibilityLevel accessModifier = VisibilityLevel.Public);

        /// <summary>
        /// Add using.
        /// </summary>
        /// <param name="usingName">Using name.</param>
        void AppendUsing(string usingName);

        /// <summary>
        /// Add a namespace.
        /// </summary>
        /// <param name="namespaceName">Namespace name.</param>
        void AppendNamespace(string namespaceName);

        /// <summary>
        /// Add a property.
        /// </summary>
        /// <param name="propertyName">Name.</param>
        /// <param name="propertyType">Type.</param>
        /// <param name="propertyAccessibility">Accessibility / access level.</param>
        void AppendProperty(string propertyName, Type propertyType, bool isArray = false, PropertyType propertyAccessibility = PropertyType.GetSet);

        /// <summary>
        /// Add an constructor
        /// </summary>
        /// <param name="code">Code</param>
        /// <param name="visibilityLevel">Visibility level</param>
        void AppendConstructor(string code = "", List<Variable>? parameters = null, VisibilityLevel visibilityLevel = VisibilityLevel.Public);

        void SaveTo(string sourceFile);

    }
}
