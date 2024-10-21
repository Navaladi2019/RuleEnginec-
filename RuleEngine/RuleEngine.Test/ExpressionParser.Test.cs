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
            var expression =  exp.Parse("a.Age == 10", [RuleParameter.Create("a",model)],typeof(bool));
        }
    }
}
