using RuleEngine.Helper;

namespace RuleEngine.Test
{
    [TestClass]
    public class Test_PropertySetter
    {
        [TestMethod]
        public void TestMethod1()
        {
            var obj = new TestSettser();
            Utils.SetProperty(obj, "Name", "Naval");
            Assert.IsTrue(obj.Name == "Naval");
        }
    }

    public class TestSettser
    {
        public string Name { get; set; }
        public string Age { get; set; }
    }
}