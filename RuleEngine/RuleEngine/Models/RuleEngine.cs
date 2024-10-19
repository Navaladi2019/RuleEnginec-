using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Models
{
    public class RuleEngine(dynamic Inputparam, ITGRuleSet RulesetParam)
    {
        public  dynamic Input { get => Inputparam; }
        public  ITGRuleSet RuleSet { get => RulesetParam; }
        public required Dictionary<string, dynamic> Ctx { get; set; } = [];
    }
}