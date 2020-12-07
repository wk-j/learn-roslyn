using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using Microsoft.CodeAnalysis;
using System.IO;
using Microsoft.CodeAnalysis.Emit;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.Immutable;

var source = File.ReadAllText("resource/Attribute.cs");
var defaultCompilationOptions =
    new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            .WithOverflowChecks(true)
            .WithPlatform(Platform.X64)
            .WithOptimizationLevel(OptimizationLevel.Debug);

var corelib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
var tree = CSharpSyntaxTree.ParseText(source);
var compilation = CSharpCompilation.Create("TestRoslyn.dll",
    new SyntaxTree[] { tree }, new[] { corelib }, defaultCompilationOptions);

var symbol = compilation.GetTypeByMetadataName("Hello.Student2");

var excludeAttribute = symbol
    .GetAttributes()
    .FirstOrDefault(x => x.AttributeClass.Name == "ExcludeAttribute");

var baseType = excludeAttribute.ConstructorArguments[0].Value as INamedTypeSymbol;
var properties = excludeAttribute.ConstructorArguments[1].Values;

var typeName = GetTypeName(baseType);
var members = GetTypeMembers(baseType);
var propertiesValue = GetPropertiesValue(properties);

Console.WriteLine("Type name = {0}", typeName);
Console.WriteLine("Type members = {0}",
    string.Join(", ", members.Select(x => $"{x.Name}({x.Type.Name})")));
Console.WriteLine("Properties = {0}", string.Join(", ", propertiesValue));

string[] GetPropertiesValue(ImmutableArray<TypedConstant> constants) {
    return constants.Select(x => x.Value as string).ToArray();
}

string GetTypeName(INamedTypeSymbol symbol) {
    return $"{symbol.ContainingNamespace}.{symbol.Name}";
}

static List<IPropertySymbol> GetTypeMembers(INamedTypeSymbol symbol) {
    var members = symbol.GetMembers()
        .Where(x => x.Kind == SymbolKind.Property)
        .OfType<IPropertySymbol>();
    return members.ToList();
}