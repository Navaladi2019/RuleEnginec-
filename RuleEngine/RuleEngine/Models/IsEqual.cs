using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Models
{
    public interface IsEqual<T> where T : class
    {
        public bool IsEqual(T obj);
    }
}
