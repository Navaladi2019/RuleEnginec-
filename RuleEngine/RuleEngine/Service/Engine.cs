using RuleEngine.ExpressionBuilders;
using RuleEngine.Models;
using System.Dynamic;

namespace RuleEngine
{
    public class Engine
    {
        public readonly Stack<ITGRule> RuleExecutionStack = new Stack<ITGRule>();
        private readonly dynamic Inputparam;
        public Engine(object Inputparam, ITGRuleSet RulesetParam)
        {
            RuleSet = RulesetParam;
            this.Inputparam = Inputparam;
            Ctx = Utils.ConvertToExpando(Inputparam);
            InitContext();
        }


        private void InitContext()
        {
            Ctx.SetRuleSetId(RuleSet.RuleSetId);
            Ctx.SetEngineStatus(RuleEngineStatus.Created);
            Ctx.SetEngineStatus(RuleEngineStatus.Created);
        }


        public readonly ITGRuleSet RuleSet;
        /// <summary>
        /// Global Context, it can be used to set object in rules and propogate from Rule A to B and so on.
        /// </summary>
        public readonly ExpandoObject Ctx;

        public RuleEngineStatus Status => Ctx.GetEngineStatus();


        public async Task ExecuteAsync()
        {
            await ExecuteRules(RuleSet.Rules);
        }

        public async Task ExecuteRules(List<ITGRule> rules)
        {
           await ExecuteRulesAsync(rules,Ctx);
        }

        public static async Task ExecuteRulesAsync(List<ITGRule> rules, ExpandoObject ctx)
        {
            if (rules == null || rules.Count < 1)
            {
                return;
            }
            foreach (var rule in rules)
            {
                try 
                {
                    var status = ctx.GetEngineStatus();
                    if (status != RuleEngineStatus.Faulted && status != RuleEngineStatus.Failed)
                    {
                        if (rule.Enabled == false)
                        {
                            rule.Status = RuleStatus.Skipped;
                            continue;
                        }
                        await rule.ExecutesAsync(ctx);
                        rule.Status = RuleStatus.Pass;
                    }
                }
                catch (Exception ex)
                {   rule.Status = RuleStatus.Error;
                    ctx.SetEngineStatus(RuleEngineStatus.Faulted);
                    break;
                }
            }
        }
    }
}