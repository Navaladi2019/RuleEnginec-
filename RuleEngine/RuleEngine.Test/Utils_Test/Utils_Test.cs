
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
            Utils.SetProperty(expando.obj, "Name", "Naval");
            Assert.IsTrue(expando.obj == obj);
        }

        [TestMethod]
        public void Test_SetPropertyByExpression()
        {
            var obj = new Utils_TestModels { Name = "Parent", Age = 10, child = new Utils_TestModels { Name="Child1",Age= 20,child = new Utils_TestModels { Name = "child2",Age= 30} } };
            var ii = new { obj };
            dynamic expando = Utils.ConvertToExpando(ii);
            Utils.SetPropertyByExpression(expando, "ctx.obj.child.Age", "ctx.obj.child.Age*2");
        }

    }
}
