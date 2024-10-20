using RuleEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.ExpressionBuilders
{
    public class ExpressionBuilderFactory
    {
        public ExpressionBuilderBase GetBuilder(ITGRule rule)
        {
            ArgumentNullException.ThrowIfNull(rule, nameof(rule));
            return rule switch
            {
                ValidationRule => new LambdaExpressionBuilder(),
                _ => throw new InvalidOperationException($"Expression builder not available for [{rule}]"),
            };
        }
    }
}
