
namespace RuleEngine
{
    public interface IFailureActionRule
    {
        public List<ITGRule> FailureRules { get; set; }
    }
}
