using RuleEngine.ExpressionBuilders;
using RuleEngine.Models;
using System;
using System.Dynamic;

namespace RuleEngine
{
    public class ValidationRule : ITGRule, IFailureActionRule, ISuccessActionRule, ILambdaExpressionRules
    {
        public string Message { get; set; } = string.Empty;
        public List<ITGRule> FailureRules { get; set; } = [];
        public List<ITGRule> SuccessRules { get; set; } = [];
        public string Expression { get; set; } = string.Empty;

        public override async Task ExecutesAsync(ExpandoObject ctx)
        {
                var parser = new RuleExpressionParser();
                var result = parser.Evaluate<bool>(Expression, [RuleParameter.Create("ctx", ctx)]);
                if (result)
                {
                    Status = RuleStatus.Pass;
                    await Engine.ExecuteRulesAsync(SuccessRules,ctx);
                }
                else
                {
                    Status = RuleStatus.Fail;
                    if (Message != null && Message != string.Empty)
                    {
                        ctx.SetRuleEngineErrMessage(Message);
                    }
                    if (ContinueOnError)
                    {
                        await Engine.ExecuteRulesAsync(FailureRules,ctx);
                    }
                    else
                    {
                        ctx.SetEngineStatus(RuleEngineStatus.Failed);
                    }
                }
        }
    }
}
