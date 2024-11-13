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

        public  string Execute(Guid RuleId,int version, string Code, List<(string Path, string DLL)> ReferenceDLLs = null)
        {
            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            CSharpCompilationOptions defaultCompilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithOverflowChecks(true)
                .WithOptimizationLevel(OptimizationLevel.Release);
            List<MetadataReference> References  = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.Location))
            .Select(a => MetadataReference.CreateFromFile(a.Location))
            .Cast<MetadataReference>()
            .ToList();
            foreach (var item in ReferenceDLLs)
            {
                References.Add(MetadataReference.CreateFromFile(Path.Combine(item.Path ?? assemblyPath, item.DLL)));
            }
            CSharpCompilation compilation = CSharpCompilation.Create(
              RuleId.ToString()+version.ToString(),
                syntaxTrees: new[] {
                CSharpSyntaxTree.ParseText(Code)
                },
                references: References,
                options: defaultCompilationOptions);
            string folderPath = Path.Combine(assemblyPath, "ITGRules");
            System.IO.Directory.CreateDirectory(folderPath);
            var filepath = Path.Combine(folderPath, $"{RuleId}.{version}.dll");
            using (var fileStream = new FileStream(Path.Combine(folderPath,  $"{RuleId}.{version}.dll"), FileMode.OpenOrCreate))
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
            return filepath;

        }
    }

   
}