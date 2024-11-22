

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using RuleEngine.Models;
using System.Dynamic;

namespace RuleEngine
{
  
    public abstract class ITGRule : IsEqual<ITGRule>
    {
        [BsonRepresentation(BsonType.String)]
        public  Guid Id {  get; set; }
       public  string Name { get; set; }
       public RuleStatus Status { get; set; } = RuleStatus.Pending;
       public bool Enabled { get; set; } = true;
        public RuleParserType RuleParserType { get;set; }
        public abstract Task ExecutesAsync(ExpandoObject ctx);
        public int Version { get; set; }
        public virtual bool IsEqual(ITGRule obj) {

            return Name == obj.Name && Enabled == obj.Enabled;
        }
    }

    public enum RuleParserType
    {
        LambdaExpression,
        CSharpEpression
    }
  
}
