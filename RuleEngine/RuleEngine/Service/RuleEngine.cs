using RuleEngine.ExpressionBuilders;
using RuleEngine.Helper;
using RuleEngine.Models;
using System.Dynamic;

namespace RuleEngine.Service
{
    public class RuleEngine
    {
        public readonly Stack<ITGRule> RuleExecutionStack = new Stack<ITGRule>();
        private readonly dynamic Inputparam;
        public RuleEngine(object Inputparam, ITGRuleSet RulesetParam)
        {
            RuleSet = RulesetParam;
            this.Inputparam = Inputparam;
            Ctx = Utils.ConvertToExpando(Inputparam);
            InitContext();
        }


        private void InitContext()
        {
            Ctx.TryAdd("ErrorMessage", new List<string>());
        }
        public readonly ITGRuleSet RuleSet;
        /// <summary>
        /// Global Context, it can be used to set object in rules and propogate from Rule A to B and so on.
        /// </summary>
        public readonly ExpandoObject Ctx;

        public RuleEngineStatus Status { get; set; }


        public async Task ExecuteAsync()
        {
            await ExecuteRules(RuleSet.Rules);
        }

        public async Task ExecuteRules(List<ITGRule> rules)
        {
            if (rules?.Count < 1) { 
                return;
            }

            for (int i = 0; i < rules.Count && (Status != RuleEngineStatus.Faulted || Status != RuleEngineStatus.Failed); i++)
            {

                if (rules[i].Enabled == false){
                    rules[i].Status = RuleStatus.Skipped;
                    continue;
                }
                if (rules[i] is ValidationRule)
                {
                    var parser = new RuleExpressionParser();
                    var result =   parser.Evaluate<bool>(rules[i].Expression, [RuleParameter.Create("ctx", Ctx)]);
                    if (result)
                    {
                        rules[i].Status = RuleStatus.Pass;
                        await ExecuteRules(rules[i].SuccessRules);
                    }
                    else
                    {
                        rules[i].Status = RuleStatus.Fail;
                        if (rules[i].Message != null && rules[i].Message != string.Empty) {
                            (Ctx.First(x => x.Key == "ErrorMessage").Value as List<string>).Add(rules[i].Message);
                         }
                        if (rules[i].ContinueOnError) {
                            await ExecuteRules(rules[i].FailureRules);
                        }
                        else
                        {
                            Status = RuleEngineStatus.Failed;
                        }
                       
                    }
                }
            }
        }
    }
}