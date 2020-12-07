using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HelloWorld {
    class Program {
        static void Main(string[] args) {
            var s = @"
            using System;
            using System.Linq;
            using System.Text;

            namespace HelloWorld {
                public class Program {
                    static void main(String[] args) {
                        Console.WriteLine(""Hello, world!"");
                    }
                }
            }
            ";

            var tree = CSharpSyntaxTree.ParseText(s);
            var root = tree.GetCompilationUnitRoot();

            var firstMember = root.Members[0];
            var hello = (NamespaceDeclarationSyntax)firstMember;
            var program = (ClassDeclarationSyntax)hello.Members[0];
            var name = program.Identifier.Value;
            Console.WriteLine(name);
        }
    }
}
