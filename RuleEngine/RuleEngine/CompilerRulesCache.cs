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

        public static T GetCachedOrBuild<T>(this ITGRule rule, ExpandoObject ctx, Func<ICacheEntry, T> factory, string appendKey = "")
        {
            return cache.GetOrCreate($"{ctx.GetyRuleSetId()}_{rule.Id}_{rule.Version}_{appendKey}", factory);
        }

        public static void ClearAllKeysStartsWith(string prefix)
        {
          var keys =  cache.Keys.Where(x => x is string && (x as string).StartsWith(prefix)).ToList();
            foreach (var key in keys) { 
            
                cache.Remove(key);
            }
        }

        public static void ClearAllCache()
        {
            cache.Clear();
        }

        public static void Clear(object key)
        {
            cache.Remove(key);
        }

    }
}
