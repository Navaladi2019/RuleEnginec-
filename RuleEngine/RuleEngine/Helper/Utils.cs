using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Helper
{
    public class Utils
    {

        // agenda for this is create a memcache ->  key, value(dynamic)
        // setter we have to create nested setter like obj1.obj2.obj3.property = ""
        public static void SetProperty<T>(T obj, string propertyName, object value)
        {

            var parameterExpression = Expression.Parameter(typeof(T), "obj");
            var propertyExpression = Expression.Property(parameterExpression, propertyName);
            var valueExpression = Expression.Parameter(typeof(object), "value");
            var convertedValue = Expression.Convert(valueExpression, propertyExpression.Type);
            var assignExpression = Expression.Assign(propertyExpression, convertedValue);
            var expression = Expression.Lambda<Action<T, object>>(assignExpression, parameterExpression, valueExpression);
            var setter = expression.Compile();
            setter(obj, value);

        }
    }
}
