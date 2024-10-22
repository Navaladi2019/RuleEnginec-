using RuleEngine.ExpressionBuilders;
using RuleEngine.Models;
using RuleEngine.Test.Utils_Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Test
{
    [TestClass]
    public class ExpressionParser
    {
        [TestMethod]
        public void TestBuildExpression()
        {
            var exp = new RuleExpressionParser();

            var model = new Utils_TestModels { Age = 10, Name = "a" };
            var model2 = new Utils_TestModels { Age = 11, Name = "a" };
            var ruleParams = new RuleParameter[] { RuleParameter.Create("a", model), RuleParameter.Create("b", model2) };
            var funcDelegate =  exp.Compile<bool>("a.Age == 10 && b.Age ==11", ruleParams);
            var result = funcDelegate.Invoke(ruleParams.Select(x=>x.Value).ToArray());
        }

        [TestMethod]
        public void Test_EvaluateExpressionAdd()
        {
            var exp = new RuleExpressionParser();

            var model = new Utils_TestModels { Age = 10, Name = "a" };
            var model2 = new Utils_TestModels { Age = 11, Name = "a" };
            var ruleParams = new RuleParameter[] { RuleParameter.Create("a", model), RuleParameter.Create("b", model2) };
            var funcDelegate = exp.Compile<dynamic>("a.Name + b.Name", ruleParams);
            var result = funcDelegate.Invoke(ruleParams.Select(x => x.Value).ToArray());
        }
    }
}
