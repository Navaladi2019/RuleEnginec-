namespace RuleEngine.Store
{
    public interface IRuleStore
    {
        Task<RuleSet> GetAsync(string id);
        Task<RuleSet> GetByNameAsync(string name);
        Task CreateAsync(RuleSet newBook, string userName);
        Task UpdateAsync(string id, RuleSet ruleSet, string userName);
        Task RemoveAsync(string id, string userName);
        Task<List<RuleSetAudit>> GetVersions(string id);
        Task RestoreToVersionAsync(string id, int version, string userName);
    }
}
