using RuleEngine.Models;
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
            ruleset.Rules.Add(new ValidationRule { Expression = "ctx.Application.Age > 65", Id = Guid.NewGuid(), Name = "Validate Age", ContinueOnError = true, Message = "Applicant age should be greateer than 65" });
            ruleset.Rules.Add(new ValidationRule { Expression = "ctx.Application.Account != null && ctx.Application.Account.Availablebalance > 0", Id = Guid.NewGuid(), Name = "Validate Availablebalance", ContinueOnError = true, Message = "Applicant Account balance should be greater than 0." });
            ruleset.Rules.Add(new ValidationRule { Expression = "ctx.ErrorMessage.Count == 0", Id = Guid.NewGuid(), Name = "Fail on Validations fail", ContinueOnError = false, Message = "" });


            var basicClass = new BasicRuleEngine { Age = 60, City = "PORTVILA", Status = "Pendingcreation", Account = new MemberAccount { Availablebalance = 1000 }, AvailableWithdrawalAmount = 0 };

            dynamic input = new {  };
            input.Application = basicClass;
            input.Age = 30;
            input.tyype = "";

            var expo = new ExpandoObject();
            (expo as IDictionary<string, object>).Add("Application", basicClass);
            var engine = new Engine(input, ruleset);
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
