using RuleEngine.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RuleEngine.Models
{
    public class RuleParameter
    {
        public RuleParameter(string name, object value)
        {
            Value = Utils.GetTypedObject(value);
            Name = name;
            Type = Value?.GetType() ?? typeof(object);
            ParameterExpression = Expression.Parameter(Type, Name);
        }

        public Type Type { get; private set; }
        public string Name { get; private set; }
        public object Value { get; private set; }
        public ParameterExpression ParameterExpression { get; private set; }

        public static RuleParameter Create(string name, object value)
        {
            return new RuleParameter(name, value);
        }

    }
}
