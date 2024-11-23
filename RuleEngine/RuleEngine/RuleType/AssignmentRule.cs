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

            var leftSide = expressions[0].Trim();
            var rightSide = expressions[1].Trim();

            if (string.IsNullOrWhiteSpace(leftSide) || string.IsNullOrWhiteSpace(rightSide)) {
                throw new Exception($"Invalid Expression {Expression} on assignment operation");
            }
            var parser = new RuleExpressionParser();
            var param = new RuleParameter[] { RuleParameter.Create("ctx", ctx) };
            var rightSideFunc = this.GetCachedOrBuild(ctx,(e)=> { return parser.Compile<object>(rightSide, param);},"Right" ) ;
            var leftSideFunc = this.GetCachedOrBuild(ctx, (e) => { return Utils.GetSetterExpression(ctx, leftSide, rightSide); }, "Left");
            var value = rightSideFunc(param.Select(x => x.Value).ToArray());
            leftSideFunc(ctx, value);
            return Task.CompletedTask;
        }

        public override bool IsEqual(ITGRule obj)
        {
            return base.IsEqual(obj) && Expression == ((AssignmentRule)obj).Expression;

        }
    }
}
