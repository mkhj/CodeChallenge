using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace CodeChallenge.Core.Compiler
{

    public static class Compiler
    {

        private static CSharpCompilationOptions GetCompilerOptions()
        {
            var compilationOptions = new CSharpCompilationOptions(
                OutputKind.DynamicallyLinkedLibrary,
                optimizationLevel: OptimizationLevel.Release
            );

            return compilationOptions;
        }

        private static PortableExecutableReference[] GetReferences()
        {
            // Add necessary references
            var references = new[]
            {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            //MetadataReference.CreateFromFile(typeof(System.Console).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location),
            MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location)
        };

            return references;
        }

        public static BuildResult BuildSolution(string code)
        {
            // Set up the compilation options
            var compilationOptions = GetCompilerOptions();

            // Add necessary references
            var references = GetReferences();

            // Compile the code
            //var codeString = SourceText.From(task.GetTaskTemplate());
            var syntaxTree = SyntaxFactory.ParseSyntaxTree(code);

            var compilation = CSharpCompilation.Create("LibraryAssembly")
                .WithOptions(compilationOptions)
                .AddReferences(references)
                .AddSyntaxTrees(syntaxTree);

            using var ms = new MemoryStream();

            var emitResult = compilation.Emit(ms);

            var buildResult = new BuildResult();

            // Handle compilation errors
            var compilerErrors = !emitResult.Success;
            if (compilerErrors)
            {
                foreach (var diagnostic in emitResult.Diagnostics)
                {
                    buildResult.AddError(diagnostic.ToString());
                }

                return buildResult;
            }

            ms.Seek(0, SeekOrigin.Begin);

            // Load the compiled assembly
            var assembly = Assembly.Load(ms.ToArray());
            buildResult.AddAssembly(assembly);

            return buildResult;
        }

    };
}

