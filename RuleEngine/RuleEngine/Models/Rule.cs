

namespace RuleEngine.Models
{
  

    public abstract class ITGRule
    {
       public abstract int Id {  get; set; }
       public abstract string Expression { get; set; }
       public abstract string Name { get; set; }
       public RuleStatus Status { get; set; }
       public bool Enabled { get; set; } = true;

        /// <summary>
        /// If the Rule Set is disabled then we will always fire OnSuccess Action
        /// </summary>
        public required RuleAction RuleAction { get; set; }

        public RuleOperator Operator {  get; set; }

        /// <summary>
        /// If the Operator is not set or null then these rules will not execute
        /// </summary>
        public List<ITGRule> Rules { get;set; } = [] ;
    }

  
}
