using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace emburradinho.tests.editmode
{
    public abstract class Test_EditMode_Abstract
    {

        private StringBuilder _sb = new StringBuilder();
        protected List<Scene> runningScenes = new List<Scene>();

        // ----------------------------------------------------------------------------------
        // ========================== Setup ============================
        // ----------------------------------------------------------------------------------

        /// <summary>
        /// Test tips
        /// * 1. Assign -- Instantiate everything needed
        /// * 2. Act -- Perform calls to modify\generate testing data
        /// * 3. Assert -- Assert if every output is as it should be
        /// </summary>
        [SetUp]
        public virtual void SetUp()
        {
            StartLogging();
            CreateTestScene();
        }

        [TearDown]
        public virtual void TearDown()
        {
            StopLogging();
        }


        // ----------------------------------------------------------------------------------
        // ========================== Logging ============================
        // ----------------------------------------------------------------------------------

        private void StartLogging()
        {
            _sb.Clear();

            System.Reflection.MethodBase methodBase = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod();
            string currentClass = string.Format("[{0}]", methodBase.ReflectedType.Name);

            Log("==== {0} Starting unit tests...", currentClass);
        }

        protected void Log(string log, params object[] args)
        {
            _sb.AppendLine(string.Format(log, args));
        }

        protected void LogCurrentTest()
        {
            System.Reflection.MethodBase methodBase = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod();
            string currentTest = string.Format("[{0}]", methodBase.Name);
            _sb.AppendLine("-- Running " + currentTest);
        }

        private void StopLogging()
        {
            System.Reflection.MethodBase methodBase = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod();
            string currentClass = string.Format("[{0}]", methodBase.ReflectedType.Name);

            Log("===== {0} Finishing unit tests...", currentClass);

            Debug.Log(_sb.ToString());
        }


        // ----------------------------------------------------------------------------------
        // ========================== Scenes Management ============================
        // ----------------------------------------------------------------------------------

        protected virtual Scene CreateTestScene(string name = "Runtime Scene", NewSceneMode mode = NewSceneMode.Single)
        {
            // Creates a new runtime empty scene, for testing
            Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, mode);
            newScene.name = name;

            if (mode == NewSceneMode.Single)
                runningScenes.Clear();

            runningScenes.Add(newScene);

            return newScene;
        }

        protected void UpdateScene(int sceneIdx, string name = "")
        {
            if (runningScenes.Count <= sceneIdx) return;

            Scene modifiedScene = runningScenes[sceneIdx];

            if (!string.IsNullOrEmpty(name))
                modifiedScene.name = name;

            runningScenes[sceneIdx] = modifiedScene;
        }

        // ----------------------------------------------------------------------------------
        // ========================== Hierarchy ============================
        // ----------------------------------------------------------------------------------

        /// <summary>
        /// Logs the Editor hierarchy in a JSON Format
        /// </summary>
        public void LogEditorHierarchy()
        {
            string hierarchy = GetEditorHierarchyJson();
            Log("Hierarchy: {0}", hierarchy);
        }

        /// <summary>
        /// Retrieve the Editor hierarchy in a JSON Format
        /// </summary>
        /// <returns>A string containing the editor hierarchy</returns>
        public string GetEditorHierarchyJson()
        {
            return JsonConvert.SerializeObject(GetEditorHierarchy());
        }

        /// <summary>
        /// Retrieve the Editor hierarchy in an Object Format
        /// </summary>
        /// <returns>A object containing the editor hierarchy</returns>
        public object GetEditorHierarchy()
        {
            object GetGameobjectNameHierarchyRecursive(GameObject go)
            {
                string[] components = go.GetComponents<Component>().Select(x => x.GetType().Name).ToArray();
                Transform transform = go.transform;

                return transform.childCount == 0
                    ? new
                    {
                        go.name,
                        components
                    }
                    : new
                    {
                        go.name,
                        components,
                        children = Enumerable.Range(0, transform.childCount)
                        .Select(transform.GetChild)
                        .Select(child => GetGameobjectNameHierarchyRecursive(child.gameObject)),
                    };
            }

            List<object> sceneList = new List<object>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                List<object> gameObjectList = new List<object>();
                Scene scene = SceneManager.GetSceneAt(i);
                GameObject[] rootGOList = scene.GetRootGameObjects();
                foreach (var rootGO in rootGOList)
                {
                    gameObjectList.Add(GetGameobjectNameHierarchyRecursive(rootGO));
                }

                sceneList.Add(new
                {
                    name = scene.name,
                    gameObjects = gameObjectList.ToArray(),
                });
            }

            object hierarchy = new
            {
                scenes = sceneList.ToArray()
            };

            return hierarchy;
        }
    }
}