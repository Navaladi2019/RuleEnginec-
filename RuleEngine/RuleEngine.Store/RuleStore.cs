using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Store
{
    public class RuleStore : IRuleStore
    {
        private readonly IMongoCollection<RuleSet> _rulesSetsCollection;
        private readonly IMongoCollection<RuleSetAudit> _rulesSetAuditCollection;

        public RuleStore(IOptions<RuleEngineDbSettings> ruleEngineDbSettings)
        {
            var mongoClient = new MongoClient(
           ruleEngineDbSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                ruleEngineDbSettings.Value.DatabaseName);

            _rulesSetsCollection = mongoDatabase.GetCollection<RuleSet>(
                ruleEngineDbSettings.Value.CollectionName ?? "RuleSet");
            _rulesSetAuditCollection = mongoDatabase.GetCollection<RuleSetAudit>(
            ruleEngineDbSettings.Value.AuditCollectionName ?? "RuleSetAudit");
        }

        public async Task<RuleSet> GetAsync(string id)
        {
            return await _rulesSetsCollection.Find(x => x.RuleSetId == id).FirstOrDefaultAsync();
        }


        public async Task<RuleSet> GetByNameAsync(string name)
        {
            return await _rulesSetsCollection.Find(x => x.Name == name).FirstOrDefaultAsync();
        }


        public async Task CreateAsync(RuleSet ruleSet, string userName)
        {

            ArgumentNullException.ThrowIfNull(ruleSet);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(userName);
            ruleSet.LastModifiedBy = userName;
            ruleSet.LastModifiedDate = DateTime.Now;
            ruleSet.Version = 1;

            var excistingRuleSet = await GetByNameAsync(ruleSet.Name);

            if (excistingRuleSet != null)
            {
                throw new Exception("Duplicate rule names are not allowed.");
            }
            await _rulesSetsCollection.InsertOneAsync(ruleSet);
        }


        public async Task UpdateAsync(string id, RuleSet ruleSet, string userName)
        {

            ArgumentNullException.ThrowIfNull(ruleSet);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(id);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(userName);

            var oldRuleSet = await GetAsync(id);

            if (oldRuleSet.IsEqual(ruleSet))
            {
                throw new Exception("No changes made");
            }

            if (ruleSet.Name != oldRuleSet.Name) {

                throw new Exception("RuleSet name Cannot be changed.");
            }

            ruleSet.Version = oldRuleSet.Version + 1;
            ruleSet.LastModifiedBy = userName;
            ruleSet.LastModifiedDate = DateTime.Now;

            await _rulesSetAuditCollection.InsertOneAsync(new RuleSetAudit
            {
                Owner = oldRuleSet.LastModifiedBy,
                RuleSet = oldRuleSet,
                RuleSetId = oldRuleSet.RuleSetId,
                Version = oldRuleSet.Version
            });

            await _rulesSetsCollection.ReplaceOneAsync(x => x.RuleSetId == id, ruleSet);
        }

        public async Task RestoreToVersionAsync(string id, int version, string userName)
        {
            var versionedRuleSet = await _rulesSetAuditCollection.Find(x => x.RuleSetId == id && x.Version == version).FirstOrDefaultAsync();
            ArgumentNullException.ThrowIfNull(versionedRuleSet, "Provided rule set with version combination not found.");
            await UpdateAsync(id, versionedRuleSet?.RuleSet, userName);
        }
        public async Task<List<RuleSetAudit>> GetVersions(string id)
        {
            return await _rulesSetAuditCollection.Find(x => x.RuleSetId == id).ToListAsync();
        }

        public async Task RemoveAsync(string id, string userName)
        {
            await _rulesSetsCollection.DeleteOneAsync(x => x.RuleSetId == id);
        }

    }
}
