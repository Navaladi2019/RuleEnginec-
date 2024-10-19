
namespace RuleEngine.Models
{
    public class ITGRuleSet
    {
        public required string Name { get; set; } = string.Empty;
        public required string Description { get; set; } = string.Empty;
        public List<ITGRule> Rules { get; set; } = [];
    }
}
