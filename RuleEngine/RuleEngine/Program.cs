using System.Linq.Expressions;

namespace RuleEngine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var a = new TestObject();
            SetProperty(a, "Name", "Naval");
            SetProperty(a, "Name", "Naval2");
        }


        public static void SetProperty<T>(T obj, string propertyName, object value)
        {
            if(!CachedSetter.TryGetValue(typeof(T).AssemblyQualifiedName + propertyName, out var  setter))
            {
                var parameterExpression = Expression.Parameter(typeof(T), "obj");
                var propertyExpression = Expression.Property(parameterExpression, propertyName);
                var valueExpression = Expression.Parameter(typeof(object), "value");
                var convertedValue = Expression.Convert(valueExpression, propertyExpression.Type);
                var assignExpression = Expression.Assign(propertyExpression, convertedValue);
                setter = Expression.Lambda<Action<T, object>>(assignExpression, parameterExpression, valueExpression).Compile() ;
                CachedSetter.Add(typeof(T).AssemblyQualifiedName + propertyName, value: setter);
            }
            setter(obj, value);
        }

        static Dictionary<string,dynamic> CachedSetter = new();
    }

    public class TestObject
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }
}
