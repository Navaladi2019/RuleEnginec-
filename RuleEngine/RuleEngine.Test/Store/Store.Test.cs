using Microsoft.Extensions.Options;
using RuleEngine.RuleType;
using RuleEngine.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Test.Store
{
    [TestClass]
    public class StoreTest
    {

        private IOptions<RuleEngineDbSettings> ruleEngineDbSettings;
        [TestInitialize]
        public void Initialize()
        {
            ruleEngineDbSettings = Options.Create(new RuleEngineDbSettings { ConnectionString = "mongodb://localhost:27017", DatabaseName = "ITGRule", AuditCollectionName = "Audit", CollectionName = "RuleSet" });
        }

        [TestMethod]
        public async Task Test_Insert()
        {
            IRuleStore store = new RuleStore(ruleEngineDbSettings);

            var inputruleset = new RuleSet
            {
                Description = "DESCRIPTION",
                Name = Guid.NewGuid().ToString(),
                Input = [],
                Rules = new List<ITGRule> {
                                new ValidationRule{
                                    Expression="1==1",
                                    Id = Guid.NewGuid(),
                                    Name="name",
                                    ContinueOnError = false,
                                    SuccessRules= new List<ITGRule>{
                                        new AssignmentRule { Expression = "ctx = ctx" ,Name="Asignmanet Test",Enabled = true,Id = Guid.NewGuid()},
                                        new CSharpCodeRule{Id= Guid.NewGuid(),Enabled= true,Code ="code comes here",Name="Charpcode",NameSpace="namespace",Version = 1}
                                    }
                                },
                                new AssignmentRule{ Name="Assignment rule parent",Enabled = true,Expression= "2==2",Id = Guid.NewGuid()} }
            };
            await store.CreateAsync(inputruleset, "navaladi.");
            var dbRecordSet = await store.GetAsync(inputruleset.RuleSetId);
            Assert.IsTrue(dbRecordSet.IsEqual(inputruleset));
        }

        [TestMethod]
        public async Task Test_Update()
        {
            IRuleStore store = new RuleStore(ruleEngineDbSettings);

            var inputruleset = new RuleSet
            {
                Description = "DESCRIPTION",
                Name = Guid.NewGuid().ToString(),
                Input = [],
                Rules = new List<ITGRule> {
                                new ValidationRule{
                                    Expression="1==1",
                                    Id = Guid.NewGuid(),
                                    Name="name",
                                    ContinueOnError = false,
                                    SuccessRules= new List<ITGRule>{
                                        new AssignmentRule { Expression = "ctx = ctx" ,Name="Asignmanet Test",Enabled = true,Id = Guid.NewGuid()},
                                        new CSharpCodeRule{Id= Guid.NewGuid(),Enabled= true,Code ="code comes here",Name="Charpcode",NameSpace="namespace",Version = 1}
                                    }
                                },
                                new AssignmentRule{ Name="Assignment rule parent",Enabled = true,Expression= "2==2",Id = Guid.NewGuid()} }
            };
            await store.CreateAsync(inputruleset, "navaladi 2");

            inputruleset.Description = "Modified";

            await store.UpdateAsync(inputruleset.RuleSetId, inputruleset, "navaladi 2");

            var dbRecordSet = await store.GetAsync(inputruleset.RuleSetId);
            Assert.IsTrue(dbRecordSet.IsEqual(inputruleset));
        }

        [TestMethod]
        public async Task Test_Restore()
        {
            IRuleStore store = new RuleStore(ruleEngineDbSettings);

            var inputruleset = new RuleSet
            {
                Description = "DESCRIPTION",
                Name = Guid.NewGuid().ToString(),
                Input = [],
                Rules = new List<ITGRule> {
                                new ValidationRule{
                                    Expression="1==1",
                                    Id = Guid.NewGuid(),
                                    Name="name",
                                    ContinueOnError = false,
                                    SuccessRules= new List<ITGRule>{
                                        new AssignmentRule { Expression = "ctx = ctx" ,Name="Asignmanet Test",Enabled = true,Id = Guid.NewGuid()},
                                        new CSharpCodeRule{Id= Guid.NewGuid(),Enabled= true,Code ="code comes here",Name="Charpcode",NameSpace="namespace",Version = 1}
                                    }
                                },
                                new AssignmentRule{ Name="Assignment rule parent",Enabled = true,Expression= "2==2",Id = Guid.NewGuid()} }
            };
            await store.CreateAsync(inputruleset, "navaladi");

            inputruleset.Description = "Modified";

            await store.UpdateAsync(inputruleset.RuleSetId, inputruleset, "navaladi 2");

            inputruleset.Description = "Modified 2";

            await store.UpdateAsync(inputruleset.RuleSetId, inputruleset, "navaladi 3");

            var dbRecordSet = await store.GetAsync(inputruleset.RuleSetId);

            await store.RestoreToVersionAsync(inputruleset.RuleSetId, 1, "navaladi 3");

            var latestRecordSet = await store.GetAsync(inputruleset.RuleSetId);

            Assert.IsTrue(latestRecordSet.Description == "DESCRIPTION");
        }

    }
}
