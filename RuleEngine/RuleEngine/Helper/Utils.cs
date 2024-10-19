using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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

        public static ExpandoObject ConvertToExpando<T>(T obj)
        {
            ExpandoObject expando = new ExpandoObject();
            var expandoDict = expando as IDictionary<string, object>;
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                if (property.CanRead)
                {
                    var value = property.GetValue(obj);
                    expandoDict[property.Name] = value;
                }
            }
            return expando;
        }
    }
}
