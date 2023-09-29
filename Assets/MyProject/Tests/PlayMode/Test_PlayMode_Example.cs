using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

namespace emburradinho.tests.playmode
{
    public class Test_PlayMode_Example : Test_PlayMode_Abstract
    {
        [Test]
        public void Test_Example()
        {
            LogCurrentTest();

            Log("Hierarchy: {0}", GetEditorHierarchyJson());

            string testStr = "a";
            Assert.AreEqual("a", testStr);
        }


        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator NewTestScriptWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}