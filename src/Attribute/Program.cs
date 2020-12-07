using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using Microsoft.CodeAnalysis;
using System.IO;
using Microsoft.CodeAnalysis.Emit;
using System.Reflection;

namespace Attribute {
    class Program {
        static void Main(string[] args) {
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

            var symbol = compilation.GetTypeByMetadataName("Student2");

            var excludeAttribute = symbol
                .GetAttributes()
                .FirstOrDefault(x => x.AttributeClass.Name == "ExcludeAttribute");

            var type = excludeAttribute.ConstructorArguments[0];
            var props = excludeAttribute.ConstructorArguments[1];

            var typeValue = type.Value as INamedTypeSymbol;
            var members = typeValue.GetMembers()
                .Where(x => x.Kind == SymbolKind.Property);

            foreach (var item in members) {
                Console.WriteLine(item);
            }
        }
    }
}
