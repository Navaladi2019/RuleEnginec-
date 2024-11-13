using System.Dynamic;

namespace RuleEngine.RuleType
{
    public class AssignmentRule : ITGRule, ILambdaExpressionRules
    {
        public string Expression { get ; set; } = string.Empty;
        public override Task ExecutesAsync(ExpandoObject ctx)
        {
            var expressions =   Expression.Split("=");

            if (expressions.Length != 2) {
                throw new Exception($"Could not parse {Expression} on assignment operation");
            }

            var leftSide = expressions[0].Trim();
            var rightSide = expressions[1].Trim();

            if (string.IsNullOrWhiteSpace(leftSide) || string.IsNullOrWhiteSpace(rightSide)) {
                throw new Exception($"Invalid Expression {Expression} on assignment operation");
            }
            Utils.SetPropertyByExpression( ctx, leftSide, rightSide );
            return Task.CompletedTask;
        }
    }
}
