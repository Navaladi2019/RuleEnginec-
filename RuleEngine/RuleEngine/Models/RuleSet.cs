
namespace RuleEngine.Models
{
    public class ITGRuleSet : IsEqual<ITGRuleSet>
    { 
        public virtual string RuleSetId { get; set; }
        public required string Name { get; set; } = string.Empty;
        public required string Description { get; set; } = string.Empty;
        public List<ITGRule> Rules { get; set; } = [];
        public int Version { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public Dictionary<string, string> Input { get; set; } = [];

        public bool IsEqual(ITGRuleSet other)
        {
            return Name == other.Name && Description == other.Description && Rules.Count == other.Rules.Count && Utils.AreSame(Rules,other.Rules);
        }
    }
}
