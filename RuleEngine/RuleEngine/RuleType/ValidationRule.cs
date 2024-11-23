using MongoDB.Bson.Serialization.Attributes;
using RuleEngine;
using RuleEngine.ExpressionBuilders;
using RuleEngine.Models;
using System;
using System.Dynamic;

namespace RuleEngine.RuleType
{
    [BsonDiscriminator("ValidationRule")]
    public class ValidationRule : ITGRule
    {
        public string Message { get; set; } = string.Empty;
        public List<ITGRule> FailureRules { get; set; } = [];
        public List<ITGRule> SuccessRules { get; set; } = [];
        public string Expression { get; set; } = string.Empty;

        /// <summary>
        /// moves to next success rule and executes
        /// </summary>
        public bool ContinueOnError { get; set; }
        public override async Task ExecutesAsync(ExpandoObject ctx)
        {
            var parser = new RuleExpressionParser();
            var param = new RuleParameter[] { RuleParameter.Create("ctx", ctx) };
            var func = this.GetCachedOrBuild(ctx, (e) => { return parser.Compile<bool>(Expression, param); });
            var result = func(param.Select(x => x.Value).ToArray());
            if (result)
            {
                Status = RuleStatus.Pass;
                await Engine.ExecuteRulesAsync(SuccessRules, ctx);
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
                    await Engine.ExecuteRulesAsync(FailureRules, ctx);
                }
                else
                {
                    ctx.SetEngineStatus(RuleEngineStatus.Failed);
                }
            }
        }
        public override bool IsEqual(ITGRule obj)
        {
            if (obj is not ValidationRule otherRule)
                return false;

            return Utils.AreSame(FailureRules.OfType<ITGRule>().ToList(), otherRule.FailureRules.OfType<ITGRule>().ToList()) &&
                   Utils.AreSame(SuccessRules.OfType<ITGRule>().ToList(), otherRule.SuccessRules.OfType<ITGRule>().ToList()) &&
                   Message == otherRule.Message &&
                   base.IsEqual(obj);

        }

    }
}
