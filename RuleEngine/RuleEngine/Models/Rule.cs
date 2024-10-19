using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Models
{
    public class ITGRuleSet
    {
        public required string Name { get; set; } = string.Empty;
        public required string Description { get; set; } =  string.Empty ;
        public List<ITGRule> Rules { get; set; } = [];
    }

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
    }

    public class RuleAction
    {
        public RuleSuccessAction OnSuccess { get; set; } = new ();
        public RuleFailAction OnFailure { get; set; } = new();
    }

    public class RuleSuccessAction 
    {
       public int? NextRuleId {  get; set; }
       public string Message { get; set; } = string.Empty;
    }


    public class RuleFailAction
    {
        public int? NextRuleId { get; set; }
        public bool ContinueOnError { get;set; }
        public string Message { get; set; } = string.Empty;
    }

    public class ValidationRule : ITGRule
    {
        public required override int Id { get ; set; }
        public required override string Expression { get;set; }
        public required override string Name { get; set ; }
    }
    public class AssignmentRule : ITGRule
    {
        public required override int Id { get; set; }
        public required override string Expression { get; set; }
        public required override string Name { get; set; }
    }

    public enum  RuleStatus
    {
        Pass,
        Fail,
        Skipped
    }
}
