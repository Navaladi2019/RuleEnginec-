using RuleEngine.Models;
using RuleEngine.RuleType;
using RuleEngine.Test.Utils_Test;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Test
{
    [TestClass]
    public class AssignmentRuleTest
    {

        [TestMethod]
        public async Task TestValidationBasicMode()
        {
            var ruleset = new ITGRuleSet { Description = "Validate Withdrawal", Name = "Validate withdrawal", Rules = [] };
            ruleset.Rules.Add(new AssignmentRule { Expression = "ctx.obj.child.Age = 999", Id = Guid.NewGuid(), Name = "Assign Age" });
           

            var obj = new Utils_TestModels { Name = "Parent", Age = 10, child = 
                new Utils_TestModels { Name = "Child1", Age = 20,
                    child = new Utils_TestModels { Name = "child2", Age = 30 } } };
            var ii = new { obj };
            var engine = new Engine(ii, ruleset);
            await engine.ExecuteAsync();
            Console.WriteLine((engine.Ctx as dynamic).ErrorMessage);
        }
    }
}
