using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Store
{
    public class RuleEngineDbSettings
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string CollectionName { get; set; } = "RuleSet";

        public string AuditCollectionName { get; set; } = "RuleSetAudit";
    }
}
