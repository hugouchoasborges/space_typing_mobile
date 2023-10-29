using NUnit.Framework;

namespace tests.editmode
{
    public class Test_EditMode_Example : Test_EditMode_Abstract
    {
        [Test]
        public void Test_Example()
        {
            LogCurrentTest();

            Log("Hierarchy: {0}", GetEditorHierarchyJson());

            string testStr = "a";
            Assert.AreEqual("a", testStr);
        }
    }
}