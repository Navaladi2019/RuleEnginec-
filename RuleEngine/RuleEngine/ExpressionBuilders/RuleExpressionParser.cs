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

namespace RuleEngine.ExpressionBuilders
{
    public class RuleExpressionParser
    {

        public Expression Parse(string expression, ParameterExpression[] parameters, Type returnType)
        {
            var config = new ParsingConfig
            {
            };

            var tree = new ExpressionParser(parameters, expression, new object[] { }, config);
            return tree.Parse(returnType);
        }

        ////public Func<object[], T> Compile<T>(string expression, RuleParameter[] ruleParams)
        ////{
        ////    var rtype = typeof(T);
        ////    if (rtype == typeof(object))
        ////    {
        ////        rtype = null;
        ////    }
        ////    var parameterExpressions = GetParameterExpression(ruleParams).ToArray();

        ////    var e = Parse(expression, parameterExpressions, rtype);
        ////    if (rtype == null)
        ////    {
        ////        e = Expression.Convert(e, typeof(T));
        ////    }
        ////    var expressionBody = new List<Expression>() { e };
        ////    var wrappedExpression = WrapExpression<T>(expressionBody, parameterExpressions, new ParameterExpression[] { });
        ////    return CompileExpression(wrappedExpression);

        ////}

    }
}
