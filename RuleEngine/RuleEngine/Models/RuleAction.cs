

namespace RuleEngine.Models
{
    public class RuleAction
    {
        public RuleSuccessAction OnSuccess { get; set; } = new();
        public RuleFailAction OnFailure { get; set; } = new();
    }

    public class RuleSuccessAction
    {
        public int? NextRuleId { get; set; }
        public string Message { get; set; } = string.Empty;
    }


    public class RuleFailAction
    {
        public int? NextRuleId { get; set; }
        public bool ContinueOnError { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
