using MongoDB.Bson.Serialization.Attributes;
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
            foreach (var rule in Rules) { 
            
               await rule.ExecutesAsync(ctx);
                if (rule.Status  == RuleStatus.Pass) { 
                    break;
                }
            }
        }

        public override bool IsEqual(ITGRule obj)
        {
           return base.IsEqual(obj) && Utils.AreSame(Rules.Cast<ITGRule>().ToList(), ((IfElseRuleSet)obj).Rules.Cast<ITGRule>().ToList()) ;
        }
    }

    public class IFElseRule : ITGRule, ILambdaExpressionRules, ISuccessActionRule
    {
        public List<ITGRule> SuccessRules { get; set; } = [];
        public string Expression { get; set; } = string.Empty;
        public IfElseRuleType Type { get; set; }

        public override async Task ExecutesAsync(ExpandoObject ctx)
        {
            var parser = new RuleExpressionParser();
            var param = new RuleParameter[] { RuleParameter.Create("ctx", ctx) };
            var func = this.GetOrCreateFunc(ctx, (e) => { return parser.Compile<bool>(Expression, param); });
            var result = Type == IfElseRuleType.ELSE || func(param);
            if (result)
            {
                Status = RuleStatus.Pass;
                await Engine.ExecuteRulesAsync(SuccessRules, ctx);
            }
            else
            {
                Status = RuleStatus.Fail;
            }
        }

        public override bool IsEqual(ITGRule obj)
        {
           return base.IsEqual(obj) && this.Expression == ((IFElseRule)obj).Expression
                &&  Utils.AreSame(SuccessRules.Cast<ITGRule>().ToList(), ((IFElseRule)obj).SuccessRules.Cast<ITGRule>().ToList());

           
        }
    }
  
    public enum IfElseRuleType
    {
        IF,
        IFELSE,
        ELSE
    }
}
