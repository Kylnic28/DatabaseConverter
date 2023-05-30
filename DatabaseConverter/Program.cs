// See https://aka.ms/new-console-template for more information

using DatabaseConverter.CodeBuilder;
using DatabaseConverter.CodeBuilder.Attributes;

CSharpCodeBuilder codeBuilder = new CSharpCodeBuilder();

codeBuilder.AppendUsing("System.IO");
codeBuilder.AppendNamespace("Database");
codeBuilder.AppendClass("Person");
codeBuilder.AppendProperty("Name", typeof(string), propertyAccessibility: PropertyType.GetInit);
codeBuilder.AppendProperty("Age", typeof(int), propertyAccessibility: PropertyType.GetInit);
codeBuilder.AppendMethod("SetAge", typeof(int), code: "if (newAge >= 1 && newAge <= 100) { this.Age = newAge; }", parameters: new List<Variable> { new("newAge", typeof(int))});

codeBuilder.SaveTo("person_file.cs");