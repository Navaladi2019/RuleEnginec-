using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using RuleEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Store
{
    public class RuleSet : ITGRuleSet
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public override string? RuleSetId { get; set; }
    
    }

    public class RuleSetAudit 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public  string? Id { get; set; }
        public string RuleSetId { get; set; }
        public int Version { get;set; }
        public string Owner { get; set; }
        public  RuleSet RuleSet { get;set;}
    }
}
