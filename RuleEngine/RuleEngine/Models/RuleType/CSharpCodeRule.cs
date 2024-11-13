using System.Data;
using System;
using System.Dynamic;
using System.Reflection;

namespace RuleEngine
{
    public class CSharpCodeRule : ITGRule
    {
        public int Version { get; set; }
        public override async Task ExecutesAsync(ExpandoObject ctx)
        {
            try
            {
                var assembly = Assembly.LoadFrom($"ITGRules/{Id}.{Version}.dll");
                var type = assembly.GetTypes().First(x => x.IsAssignableFrom(typeof(ICSharpCode)));
                ICSharpCode obj = (Activator.CreateInstance(type) as ICSharpCode)!;
                var result = await obj.ExecutesAsync(ctx);
                Status = result.Status;
            }
            catch (Exception ex) { 
                Status = RuleStatus.Error;
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
