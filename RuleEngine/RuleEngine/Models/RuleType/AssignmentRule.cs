using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Models
{
    public class AssignmentRule : ITGRule
    {
        public required override int Id { get; set; }
        public required override string Expression { get; set; }
        public required override string Name { get; set; }
    }
}
