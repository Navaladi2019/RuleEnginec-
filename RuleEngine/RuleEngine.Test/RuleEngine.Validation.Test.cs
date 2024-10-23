using RuleEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Test
{
    [TestClass]
    public class RuleEngineValidation
    {
        [TestMethod]
        public async Task TestValidationBasicMode()
        {
            var ruleset = new ITGRuleSet { Description = "Validate Withdrawal", Name = "Validate withdrawal", Rules = [] };
            ruleset.Rules.Add(new ValidationRule { Expression = "ctx.Application.Age > 65", Id = 1, Name = "Validate Age", ContinueOnError = true, Message = "Applicant age should be greateer than 65" });
            ruleset.Rules.Add(new ValidationRule { Expression = "ctx.Application.Account.Availablebalance > 0", Id = 1, Name = "Validate Availablebalance", ContinueOnError = true, Message = "Applicant Account balance should be greater than 0." });
            ruleset.Rules.Add(new ValidationRule { Expression = "ctx.ErrorMessage.Count == 0", Id = 1, Name = "Fail on Validations fail", ContinueOnError = false, Message = "" });


            var basicClass = new BasicRuleEngine { Age = 60, City = "PORTVILA", Status = "Pendingcreation", Account = new MemberAccount { Availablebalance = 1000 }, AvailableWithdrawalAmount = 0 };

            var input = new { Application = basicClass };
            var engine = new RuleEngine.Service.RuleEngine(input, ruleset);
            await engine.ExecuteAsync();
            Console.WriteLine((engine.Ctx as dynamic).ErrorMessage);

        }
      

        
    }

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
