using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using RuleEngine.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine
{
    public static class CompilerRulesCache
    {
        private static readonly MemoryCache cache = new MemoryCache(Options.Create(new MemoryCacheOptions()));

        public static dynamic? Get(object key)
        {
            return cache.Get(key);
        }

        public static Func<object[], bool>? GetOrCreateFunc(this ITGRule rule,ExpandoObject ctx, Func<ICacheEntry, Func<object[], bool>> factory)
        {
            return cache.GetOrCreate($"{ctx.GetyRuleSetId()}_{rule.Id}_{rule.Version}", factory);
        }

    }
}
