using RuleEngine.Models;
using RuleEngine.RuleType;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace RuleEngine.Test
{
    [TestClass]
    public class RuleEngineValidation
    {
        [TestMethod]
        public async Task TestValidationBasicMode()
        {
            var ruleset = new ITGRuleSet { Description = "Validate Withdrawal", Name = "Validate withdrawal", Rules = [] };
            ruleset.Rules.Add(new IfElseRuleSet {  Id = Guid.NewGuid(), Name = "Validate Age", 
                Rules = new List<IFElseRule> { new IFElseRule { Expression="ctx.Application.Age == 30", Type = IfElseRuleType.IF,
                    SuccessRules = new List<ITGRule> { new AssignmentRule { Expression = "ctx.Application.Age = 55"} } },
                new IFElseRule { Expression="", Type = IfElseRuleType.ELSE,
                    SuccessRules = new List<ITGRule> { new AssignmentRule { Expression = "ctx.Application.Age = 99"} } }} });



            var basicClass = new BasicRuleEngine { Age = 60, City = "PORTVILA", Status = "Pendingcreation", Account = new MemberAccount { Availablebalance = 1000 }, AvailableWithdrawalAmount = 0 };

      

            var expo = new ExpandoObject();
            (expo as IDictionary<string, object>).Add("Application", basicClass);
            var engine = new Engine(new {Application = basicClass}, ruleset);
            await engine.ExecuteAsync();
            Console.WriteLine((engine.Ctx as dynamic).ErrorMessage);

        }
      

        
    }


    /// <summary>
    /// CREATE NEW (ruleset), APPROVE (RULE) , CANCEL (RULE) , REJECT (RULE)
    /// </summary>

    public class BasicRuleEngine
    {
        public int Id { get;set; }
        public int Age { get; set; }
        public string City { get; set; }
        public string Status { get; set; }

        public MemberAccount Account { get; set; }

        public decimal AvailableWithdrawalAmount { get; set; }
     

    }
    public class MemberAccount
    {
        public decimal Availablebalance { get; set; }
        public decimal InterestRate { get; set; }
    }
}
