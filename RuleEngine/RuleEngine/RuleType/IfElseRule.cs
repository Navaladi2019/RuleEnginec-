using MongoDB.Bson.Serialization.Attributes;
using RuleEngine;
using RuleEngine.ExpressionBuilders;
using RuleEngine.Models;
using System.Dynamic;

namespace RuleEngine
{
    [BsonDiscriminator("IfElseRuleSet")]
    public class IfElseRuleSet : ITGRule
    {
        public List<IFElseRule> Rules { get; set; } = [];

        public override async Task ExecutesAsync(ExpandoObject ctx)
        {
            foreach (var rule in Rules)
            {

                await Engine.ExecuteRulesAsync([rule],ctx);
                if (rule.Status == RuleStatus.Pass)
                {
                    break;
                }
            }
        }

        public override bool IsEqual(ITGRule obj)
        {
            return base.IsEqual(obj) && Utils.AreSame(Rules.Cast<ITGRule>().ToList(), ((IfElseRuleSet)obj).Rules.Cast<ITGRule>().ToList());
        }
    }

    public class IFElseRule : ITGRule
    {
        public List<ITGRule> SuccessRules { get; set; } = [];
        public string Expression { get; set; } = string.Empty;
        public IfElseRuleType Type { get; set; }

        public override async Task ExecutesAsync(ExpandoObject ctx)
        {
           
            if (await GetResult(ctx))
            {
                Status = RuleStatus.Pass;
                await Engine.ExecuteRulesAsync(SuccessRules, ctx);
            }
            else
            {
                Status = RuleStatus.Fail;
            }
        }

        private async ValueTask<bool> GetResult(ExpandoObject ctx)
        {
            if (Type == IfElseRuleType.ELSE)
            {
                return true;
            }
            var parser = new RuleExpressionParser();
            var param = new RuleParameter[] { RuleParameter.Create("ctx", ctx) };
            var func = this.GetCachedOrBuild(ctx, (e) => { return parser.Compile<bool>(Expression, param); });
            return func(param.Select(x => x.Value).ToArray());

        }

        public override bool IsEqual(ITGRule obj)
        {
            return base.IsEqual(obj) && Expression == ((IFElseRule)obj).Expression
                 && Utils.AreSame(SuccessRules.Cast<ITGRule>().ToList(), ((IFElseRule)obj).SuccessRules.Cast<ITGRule>().ToList());


        }
    }

    public enum IfElseRuleType
    {
        IFELSE,
        ELSE
    }
}
