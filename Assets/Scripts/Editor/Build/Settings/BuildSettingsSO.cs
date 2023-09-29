using tools;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using tools.attributes;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;

namespace build
{
    public class BuildSettingsSO : ScriptableObject
    {
        public static BuildSettingsSO Instance => MenuExtensions.LoadSOFromResources<BuildSettingsSO>(Path.Combine(CONFIG_FILE_ROOT, CONFIG_FILE_PATH));

        public string[] BuildScenes => BuildScenesHolder.Select(scene => scene.Scene).ToArray();

        [LabelText("Scenes Included in Build")]
        [PropertySpace(SpaceBefore = 10, SpaceAfter = 20)]
        [ListDrawerSettings(Expanded = true, ShowIndexLabels = false, ShowItemCount = false, DraggableItems = true)]
        [SerializeField]
        internal List<BuildSceneHolder> BuildScenesHolder;

        [TabGroup(nameof(BuildSettingsSO), "Windows x64"), HideLabel]
        public BuildStandaloneSettings StandaloneBuild;

        [TabGroup(nameof(BuildSettingsSO), "Windows x64 Debug"), HideLabel]
        public BuildStandaloneSettings StandaloneDebugBuild;

        [TabGroup(nameof(BuildSettingsSO), "Android"), HideLabel]
        public BuildAndroidSettings AndroidBuild;

        //[TabGroup(nameof(BuildSettingsSO), "Android Debug"), HideLabel]
        //public BuildAndroidSettings AndroidDebugBuild;

        [TabGroup(nameof(BuildSettingsSO), "WebGL"), HideLabel]
        public BuildWebGLSettings WebGLBuild;

        [TabGroup(nameof(BuildSettingsSO), "WebGL Debug"), HideLabel]
        public BuildWebGLSettings WebGLDebugBuild;
        public BuildPlatformSettings GetBuildSettings(bool debug = false)
        {
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                default:
                    return debug ? StandaloneDebugBuild : StandaloneBuild;

                case BuildTarget.WebGL:
                    return debug ? WebGLDebugBuild : WebGLBuild;

                //case BuildTarget.Android:
                //    return debug ? AndroidDebugBuild : AndroidBuild;

            }
        }

        public const string CONFIG_FILE_ROOT = "Assets/MyProject/Resources/Settings/Build";
        public const string CONFIG_FILE_PATH = "build_settings.asset";

        public const string BUILD_NUMBER_FORMAT = @"\d\d\d";

        private static string GetPadNumber(int number) => number.ToString().PadLeft(3, '0');

        public static string GetBuildPath(BuildPlatformSettings settings)
        {
            string targetPath = settings.BuildRootFromDataPath.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
            int buildNumber = 1;
            string buildNumberPath = GetPadNumber(buildNumber);
            if (Directory.Exists(targetPath))
            {
                string[] subfolders = Directory.GetDirectories(targetPath, "*", SearchOption.TopDirectoryOnly).Where(sub => Regex.IsMatch(sub, @"\d\d\d")).ToArray();
                foreach (string subfolder in subfolders)
                {
                    if (Path.GetFileName(subfolder).StartsWith(buildNumberPath))
                    {
                        buildNumber++;
                        buildNumberPath = GetPadNumber(buildNumber);
                    }
                }
            }

            targetPath = Path.Combine(targetPath, buildNumberPath);
            if (!string.IsNullOrEmpty(settings.Notes.Summary)) targetPath += " - " + settings.Notes.Summary;
            return targetPath;
        }

        // ----------------------------------------------------------------------------------
        // ========================== Menu Items ============================
        // ----------------------------------------------------------------------------------

        [MenuItem("MyProject/Build Settings", priority = 1000)]
        public static void MenuItem_BuildSettings()
        {
            MenuExtensions.PingOrCreateSO<BuildSettingsSO>(Path.Combine(CONFIG_FILE_ROOT, CONFIG_FILE_PATH), minSize: new Vector2(680, 480));
        }
    }

    [System.Serializable]
    internal class BuildSceneHolder
    {
        [Scene]
        [HideLabel]
        public string Scene;
    }
}
