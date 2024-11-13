using System.Dynamic;

namespace RuleEngine.RuleType
{
    public class AssignmentRule : ITGRule, ILambdaExpressionRules
    {
        public string Expression { get ; set; } = string.Empty;
        public override Task ExecutesAsync(ExpandoObject ctx)
        {
            throw new NotImplementedException();
        }
    }
}
