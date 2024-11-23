using System.Data;
using System;
using System.Dynamic;
using System.Reflection;
using System.Linq.Expressions;
using MongoDB.Bson.Serialization.Attributes;
using System.Numerics;
using RuleEngine.Compiler;


namespace RuleEngine
{
    [BsonDiscriminator("CSharpCodeRule")]
    public class CSharpCodeRule : ITGRule
    {
        public string NameSpace { get; set; }
        public string Code { get; set; }
        public List<(string Path, string DLL)> ReferenceDLLs { get; set; } = [];
        public override async Task ExecutesAsync(ExpandoObject ctx)
        {

                var instance = GetRuleInstance();
                var result = await instance.ExecutesAsync(ctx);
                Status = result.Status;
        }

        private Assembly GetAssembly()
        {
            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string folderPath = Path.Combine(assemblyPath, "ITGRules");
            var fileName = $"{Id}.{Version}.dll";
            var filepath = Path.Combine(folderPath, fileName);
            try
            {
                return Assembly.LoadFrom(filepath);
            }
            catch (FileNotFoundException ex)
            {
                CSharpCompiler.Execute(fileName, folderPath, GetConstructedCode(), ReferenceDLLs);
                return Assembly.LoadFrom(filepath);
            }
        }

        private ICSharpCode GetRuleInstance()
        {
            var assembly = GetAssembly();
            var type = assembly.GetType("CustomCSharpRule.Rule");
            return (Activator.CreateInstance(type) as ICSharpCode)!;
        }

        public override bool IsEqual(ITGRule obj)
        {
            return base.IsEqual(obj) && Version == ((CSharpCodeRule)obj).Version
                && NameSpace == ((CSharpCodeRule)obj).NameSpace
                && Code == ((CSharpCodeRule)obj).Code;
        }

        private string GetConstructedCode()
        {

            string nameSpaces = NameSpace + "\n" + "" + "\n";

            return @$"{nameSpaces}
                      using RuleEngine;
                      using System.Data;
                      using System;
                      using System.Linq;
                      using  System.Dynamic;
                      using System.Threading.Tasks;
                      namespace CustomCSharpRule
                      {{
                              public class Rule : RuleEngine.ICSharpCode
                              {{
                                  public async Task<CSharpCodeExecResult> ExecutesAsync(ExpandoObject ctx)
                                      {{
                                          var result = new CSharpCodeExecResult();
                                          {Code};
                                          if (result.Status == RuleStatus.Pending) 
                                             {{ 
                                                  result.Status = RuleStatus.Pass;
                                             }}
                                          return result;
                                       }}
                              }}
                      }}";
        }
    }


    public class CSharpCodeTemplate : ICSharpCode
    {
        public async Task<CSharpCodeExecResult> ExecutesAsync(ExpandoObject ctx)
        {
            {
                var result = new CSharpCodeExecResult { Status = RuleStatus.Pending };

                if (result.Status == RuleStatus.Pending)
                {
                    result.Status = RuleStatus.Pass;
                }
                return result;
            }
        }
    }

    public interface ICSharpCode
    {
        Task<CSharpCodeExecResult> ExecutesAsync(ExpandoObject ctx);
    }

    public class CSharpCodeExecResult
    {
        public RuleStatus Status { get; set; }
    }

}
