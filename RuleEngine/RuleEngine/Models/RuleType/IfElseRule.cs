using RuleEngine.ExpressionBuilders;
using RuleEngine.Models;
using System.Dynamic;

namespace RuleEngine
{
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
    }

    public class IFElseRule : ITGRule, ILambdaExpressionRules, ISuccessActionRule
    {
        public List<ITGRule> SuccessRules { get; set; } = [];
        public string Expression { get; set; } = string.Empty;
        public IfElseRuleType Type { get; set; }

        public override async Task ExecutesAsync(ExpandoObject ctx)
        {
            var parser = new RuleExpressionParser();
            var result = Type == IfElseRuleType.ELSE || parser.Evaluate<bool>(Expression, [RuleParameter.Create("ctx", ctx)]);
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
    }
  
    public enum IfElseRuleType
    {
        IF,
        IFELSE,
        ELSE
    }
}
