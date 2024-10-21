

namespace RuleEngine.Models
{
  
    public abstract class ITGRule
    {
       public abstract int Id {  get; set; }
        /// <summary>
        /// Expression is always string type, may be in future we can try to cha
        /// </summary>
       public abstract string Expression { get; set; }
       public abstract string Name { get; set; }
       public RuleStatus Status { get; set; }
       public bool Enabled { get; set; } = true;

        public List<ITGRule> SuccessRules { get; set; } = [];

        /// <summary>
        /// moves to next success rule and executes
        /// </summary>
        public bool ContinueOnError { get; set; }
        public string Message { get; set; } = string.Empty;

        public List<ITGRule> FailureRules { get; set; } = [];

    }

  
}
