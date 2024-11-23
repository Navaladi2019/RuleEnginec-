using RuleEngine.Models;
using RuleEngine.RuleType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Test.Utils_Test
{
    public class Utils_TestModels
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public ValidationRule Rule { get; set; }
        public Utils_TestModels child { get; set; }
    }
}
