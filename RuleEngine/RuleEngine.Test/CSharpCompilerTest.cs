using RuleEngine.Compiler;
using RuleEngine.PlugIn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Test
{
    [TestClass]
    public class CSharpCompilerTest
    {
        [TestMethod]
        public void CompileBasicCCharpCode_Tests()
        {
            var assemblyname = Guid.NewGuid();
            var folderName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ITGRule");
            var filename = "1.dll"; ;
            var filepath = Path.Combine(folderName, filename);
           CSharpCompiler.Execute( filename, folderName, "using System;\r\n\t\t\t\t\t\r\npublic class Program\r\n{\r\n\tpublic static int Main()\r\n\t{\r\n\t\tConsole.WriteLine(\"Hello World\");\r\n\t return 5;}\r\n}", []);
            var assembly = Assembly.LoadFile(filepath);
            var type = assembly.GetType("Program");
            var method = type.GetMethod("Main");

            // Create an instance of the type and invoke the method
            var instance = Activator.CreateInstance(type);
            var result = method.Invoke(instance, null);
            Assert.AreEqual<int>((int)result!, 5);;

            filename = "2.dll";
            CSharpCompiler.Execute( filename, folderName, "using System;\r\n\t\t\t\t\t\r\npublic class Program\r\n{\r\n\tpublic static int Main()\r\n\t{\r\n\t\tConsole.WriteLine(\"Hello World\");\r\n\t return 6;}\r\n}", []);
            var context2 = new PluginLoadContext();
            filepath = Path.Combine(folderName, filename);
            var assembly2 = context2.LoadFromAssemblyPath(filepath);
            var type2 = assembly2.GetType("Program");
            var method2 = type2.GetMethod("Main");

            // Create an instance of the type and invoke the method
            var instance2 = Activator.CreateInstance(type2);
            var result2 = method2.Invoke(instance2, null);
            Assert.AreEqual<int>((int)result2!,6 );
        }
    }
}
