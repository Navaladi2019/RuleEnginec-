using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core.Parser;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using RuleEngine.Models;

namespace RuleEngine.ExpressionBuilders
{
    public class RuleExpressionParser
    {

        public Expression Parse(string expression, RuleParameter[] parameters, Type returnType)
        {
            var config = new ParsingConfig
            {
            };

            var parser = new ExpressionParser(parameters.Select(x=>x.ParameterExpression).ToArray(), expression, new object[] { }, config);
            return parser.Parse(returnType);
        }

        public Func<object[], T> Compile<T>(string expression, RuleParameter[] ruleParams)
        {
            var rtype = typeof(T);
            if (rtype == typeof(object))
            {
                rtype = null;
            }
            var parameterExpression =  ruleParams.Select(x => x.ParameterExpression).ToArray();
            var e = Parse(expression, ruleParams, rtype);
            if (rtype == null)
            {
                e = Expression.Convert(e, typeof(T));
            }
            var expressionBody = new List<Expression>() { e };
            var wrappedExpression = WrapExpression<T>(expressionBody, parameterExpression, new ParameterExpression[] { });
            return wrappedExpression.Compile();

        }
        public T Evaluate<T>(string expression, RuleParameter[] ruleParams)
        {
           var func = Compile<T>(expression, ruleParams);
           return func(ruleParams.Select(x => x.Value).ToArray());

        }
        private static Expression<Func<object[], T>> WrapExpression<T>(List<Expression> expressionList, ParameterExpression[] parameters, ParameterExpression[] variables)
        {
            var argExp = Expression.Parameter(typeof(object[]), "args");
            var paramExps = parameters.Select((c, i) => {
                var arg = Expression.ArrayAccess(argExp, Expression.Constant(i));
                return (Expression)Expression.Assign(c, Expression.Convert(arg, c.Type));
            });
            var blockExpSteps = paramExps.Concat(expressionList);
            var blockExp = Expression.Block(parameters.Concat(variables), blockExpSteps);
            return Expression.Lambda<Func<object[], T>>(blockExp, argExp);
        }


    }
}
