

using System.Dynamic;

namespace RuleEngine
{
  
    public abstract class ITGRule
    {
       public  Guid Id {  get; set; }
       public  string Name { get; set; }
       public RuleStatus Status { get; set; } = RuleStatus.Pending;
       public bool Enabled { get; set; } = true;

        /// <summary>
        /// moves to next success rule and executes
        /// </summary>
        public bool ContinueOnError { get; set; }
        public RuleParserType RuleParserType { get;set; }
        public abstract Task ExecutesAsync(ExpandoObject ctx);

    }

    public enum RuleParserType
    {
        LambdaExpression,
        CSharpEpression
    }
  
}
