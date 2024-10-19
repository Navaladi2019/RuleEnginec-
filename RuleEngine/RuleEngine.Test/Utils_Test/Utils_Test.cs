using RuleEngine.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Test.Utils_Test
{
    [TestClass]
    public class Utils_Test
    {
        [TestMethod]
        public void Test_Setter_Simple()
        {
            var obj = new Utils_TestModels();
            Utils.SetProperty(obj, "Name", "Naval");
            Assert.IsTrue(obj.Name == "Naval");
        }

        [TestMethod]
        public void Test_ConvertObject_To_ExpandoObject()
        {
            var obj = new Utils_TestModels();
            var ii = new { obj };
            dynamic expando =  Utils.ConvertToExpando(ii);
            Assert.IsTrue(expando.obj == obj);
            Utils.SetProperty(expando.obj, "Name", "Naval");

        }
    }
}
