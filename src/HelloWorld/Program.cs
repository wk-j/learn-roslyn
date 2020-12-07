using System;
using System.IO;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HelloWorld {
    class Program {
        static void Main(string[] args) {
            var source = File.ReadAllText("resource/HelloWorld.cs");
            var tree = CSharpSyntaxTree.ParseText(source);
            var root = tree.GetCompilationUnitRoot();

            var firstMember = root.Members[0];
            var hello = (NamespaceDeclarationSyntax)firstMember;
            var program = (ClassDeclarationSyntax)hello.Members[0];
            var name = program.Identifier.Value;
            Console.WriteLine(name);
        }
    }
}
