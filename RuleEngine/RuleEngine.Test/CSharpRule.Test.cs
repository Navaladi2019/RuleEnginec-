using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Test
{
    [TestClass]
    public class CSharpRule_Tests
    {
        [TestMethod]
        public void Is_CompilationSuccess()
        {
            var rule = new CSharpCodeRule { Id = Guid.NewGuid(),Version=1,NameSpace= "using System;" ,Code= "Console.WriteLine(\"Hello World\");" };
            var result =   rule.ExecutesAsync(new System.Dynamic.ExpandoObject());
            Assert.IsTrue(rule.Status == RuleStatus.Pass);
        }

        [TestMethod]
        public void Is_Rule_Error_OnException()
        {
            var rule = new CSharpCodeRule { Id = Guid.NewGuid(), Version = 1, NameSpace = "using System;", Code = "throw new Exception();" };
            var result = rule.ExecutesAsync(new System.Dynamic.ExpandoObject());
            Assert.IsTrue(rule.Status == RuleStatus.Error);
        }

        [TestMethod]
        public void Is_Rule_Fail_SetOnCustomCode()
        {
            var rule = new CSharpCodeRule { Id = Guid.NewGuid(), Version = 1, NameSpace = "using System;", Code = "result.Status = RuleStatus.Fail" };
            var result = rule.ExecutesAsync(new System.Dynamic.ExpandoObject());
            Assert.IsTrue(rule.Status == RuleStatus.Fail);
        }

        [TestMethod]
        public void Is_Rule_Pass_OnNotSettingStatus()
        {
            var rule = new CSharpCodeRule { Id = Guid.NewGuid(), Version = 1, NameSpace = "using System;", Code = "" };
            var result = rule.ExecutesAsync(new System.Dynamic.ExpandoObject());
            Assert.IsTrue(rule.Status == RuleStatus.Pass);
        }
    }
}
