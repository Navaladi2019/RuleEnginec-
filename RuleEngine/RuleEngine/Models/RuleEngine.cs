using RuleEngine.Helper;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Models
{
    public class RuleEngine
    {
        public RuleEngine(dynamic Inputparam, ITGRuleSet RulesetParam)
        {
            this.RuleSet= RulesetParam;
            this.Ctx = Utils.ConvertToExpando(Inputparam);
        }
        public readonly ITGRuleSet RuleSet;
        public required ExpandoObject Ctx { get; set; } = [];
    }
}