
namespace RuleEngine.Models
{
    public enum RuleOperator
    {
        None,
        /// <summary>
        ///  A bitwise or logical AND operation, such as (a & b) in C# and (a And b)
        /// </summary>
        And,
        /// <summary>
        /// A conditional AND operation that evaluates the second operand only if the firstoperand evaluates to true. It corresponds to (a && b) in C# and (a AndAlso b)
        /// </summary>
        AndAlso,
        /// <summary>
        /// A bitwise or logical OR operation, such as (a | b) in C# or (a Or b)
        /// </summary>
        Or,
        /// <summary>
        /// A short-circuiting conditional OR operation, such as (a || b) in C# or (a OrElse b)
        /// </summary>
        OrElse,
    }

}
