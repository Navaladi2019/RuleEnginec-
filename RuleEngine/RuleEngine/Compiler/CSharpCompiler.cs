using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using RuleEngine.PlugIn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Compiler
{
    public class CSharpCompiler
    {

        public static  void Execute(string filename,string folderPath, string Code, List<(string Path, string DLL)> ReferenceDLLs = null)
        {
            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            CSharpCompilationOptions defaultCompilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithOverflowChecks(true)
                .WithOptimizationLevel(OptimizationLevel.Release);
            var assemblies = new HashSet<Assembly> { Assembly.GetExecutingAssembly() };
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                assemblies.Add(assembly);
            }
            

            List<MetadataReference> References  = assemblies
            .Where(a =>  !string.IsNullOrWhiteSpace(a.Location))
            .Select(a => MetadataReference.CreateFromFile(a.Location))
            .Cast<MetadataReference>()
            .ToList();
            foreach (var item in ReferenceDLLs)
            {
                References.Add(MetadataReference.CreateFromFile(Path.Combine(item.Path ?? assemblyPath, item.DLL)));
            }
            CSharpCompilation compilation = CSharpCompilation.Create(
              filename.Split(".")[0],
                syntaxTrees: new[] {
                CSharpSyntaxTree.ParseText(Code)
                },
                references: References,
                options: defaultCompilationOptions);
            System.IO.Directory.CreateDirectory(folderPath);
            
            using (var fileStream = new FileStream(Path.Combine(folderPath,filename), FileMode.OpenOrCreate))
            {
                var result = compilation.Emit(fileStream);

                if (!result.Success)
                {
                    // Handle compilation errors
                    var failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    List<string> sb = new();

                    foreach (Diagnostic diagnostic in failures)
                        sb.Add(diagnostic.GetMessage());
                    Exception ex = new(sb.ToString());
                    throw ex;
                }
               
            }

        }
    }

   
}