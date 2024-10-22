using RuleEngine.ExpressionBuilders;
using RuleEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Test
{
    [TestClass]
    public class ExpressionBuilder_Test
    {

        [TestMethod]
        public void ExpressionBuilder_Tests()
        {
            var builder = new ExpressionBuilderFactory();
            ITGRule rule = new ValidationRule { Expression="",Id=1,Name="Validation Rule"};
            var LambdaExpressionBuilder = builder.GetBuilder(rule);
            Assert.IsInstanceOfType<LambdaExpressionBuilder>(LambdaExpressionBuilder);
            Assert.ThrowsException<ArgumentNullException>(()=>builder.GetBuilder(null));
        }
    }
}
