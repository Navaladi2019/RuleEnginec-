using MongoDB.Bson.Serialization.Attributes;
using RuleEngine.ExpressionBuilders;
using RuleEngine.Models;
using System.Dynamic;

namespace RuleEngine
{
    [BsonDiscriminator("AssignmentRule")]
    public class AssignmentRule : ITGRule
    {
        public string Expression { get ; set; } = string.Empty;
        public override Task ExecutesAsync(ExpandoObject ctx)
        {
            var expressions =   Expression.Split("=");

            if (expressions.Length != 2) {
                throw new Exception($"Could not parse {Expression} on assignment operation");
            }

            var leftExpression = expressions[0].Trim();
            var rightExpression = expressions[1].Trim();

            if (string.IsNullOrWhiteSpace(leftExpression) || string.IsNullOrWhiteSpace(rightExpression)) {
                throw new Exception($"Invalid Expression {Expression} on assignment operation");
            }
            var parser = new RuleExpressionParser();
            var param = new RuleParameter[] { RuleParameter.Create("ctx", ctx) };
            var rightFunc = this.GetCachedOrBuild(ctx,(e)=> { return parser.Compile<object>(rightExpression, param);},"Right" ) ;
            var leftFunc = this.GetCachedOrBuild(ctx, (e) => { return Utils.GetSetterExpression(ctx, leftExpression, rightExpression); }, "Left");
            var value = rightFunc(param.Select(x => x.Value).ToArray());
            leftFunc(ctx, value);
            return Task.CompletedTask;
        }

        public override bool IsEqual(ITGRule obj)
        {
            return base.IsEqual(obj) && Expression == ((AssignmentRule)obj).Expression;

        }
    }
}
