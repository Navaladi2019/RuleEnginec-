using System.Data;
using System;
using System.Dynamic;
using System.Reflection;
using RuleEngine.RuleType;
using System.Linq.Expressions;
using MongoDB.Bson.Serialization.Attributes;
using System.Numerics;

namespace RuleEngine
{
    [BsonDiscriminator("CSharpCodeRule")]
    public class CSharpCodeRule : ITGRule
    {
        public string NameSpace { get; set; }
        public string Code { get; set; }
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
            catch (Exception ex)
            {
                Status = RuleStatus.Error;
            }
        }

        public override bool IsEqual(ITGRule obj)
        {
            return base.IsEqual(obj) && Version == ((CSharpCodeRule)obj).Version
                && NameSpace == ((CSharpCodeRule)obj).NameSpace
                && Code == ((CSharpCodeRule)obj).Code;
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
