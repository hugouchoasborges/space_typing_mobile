using log;
using Sirenix.OdinInspector;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace build
{
    [System.Serializable]
    public abstract class BuildPlatformSettings
    {
        public virtual BuildTarget Target => BuildTarget.NoTarget;
        public virtual BuildTargetGroup TargetGroup => BuildTargetGroup.Unknown;

        public BuildOptions BuildOptions;


        [ShowIf("@!FolderExists(BuildRootFromDataPath)")]
        [Button("Create Folder", ButtonSizes.Small)]
        [PropertyOrder(99)]
        private void CreateFolder() => CretateFolder(Path.Combine(Application.dataPath, "..", BuildRootFromDataPath));

        [FolderPath(RequireExistingPath = true)]
        [SerializeField]
        public string BuildRootFromDataPath = "Builds";

        [PropertySpace(SpaceBefore = 10, SpaceAfter = 10)]
        [PropertyOrder(100)]
        [BoxGroup("Notes", ShowLabel = false), HideLabel]
        public BuildNotes Notes;

        private static bool FolderExists(string path) => Directory.Exists(path);
        private static void OpenFolder(string path)
        {
            path = path
                .Replace('\\', Path.DirectorySeparatorChar)
                .Replace('/', Path.DirectorySeparatorChar);
            ELog.Log(ELogType.BUILD, "Opening Folder: {0}", path);
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                Arguments = path,
                FileName = "explorer.exe",
            };

            Process.Start(startInfo);
        }

        private static void CretateFolder(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        [ShowIf("@FolderExists(BuildRootFromDataPath)")]
        [Button("Open Build Folder", ButtonSizes.Small)]
        [PropertyOrder(101)]
        private void OpenBuildFolder() => OpenFolder(Path.Combine(Application.dataPath, "..", BuildRootFromDataPath));

        [ShowIf("@this.Target == EditorUserBuildSettings.activeBuildTarget")]
        [Button("Build", ButtonSizes.Large)]
        [PropertyOrder(102)]
        private void Build()
        {
            if (EditorUtility.DisplayDialog("BUILD", "Build starting for " + Target, "Start Build", "Cancel"))
            {
                ELog.Log_Debug(ELogType.BUILD, "Starting Build for {0}", Target);
                BuildUtils.Build(this);
            }
        }

        [ShowIf("@this.Target != EditorUserBuildSettings.activeBuildTarget")]
        [Button("Switch Platform", ButtonSizes.Large)]
        [PropertyOrder(102)]
        private void SwitchPlatform()
        {
            if (EditorUtility.DisplayDialog("Switch Platform", "Switching Platform to " + Target, "Switch Platform", "Cancel"))
            {
                ELog.Log_Debug(ELogType.BUILD, "Switching Platform to {0}", Target);
                EditorUserBuildSettings.SwitchActiveBuildTarget(TargetGroup, Target);
            }
        }
    }

    [System.Serializable]
    public class BuildNotes
    {
        [ValidateInput(nameof(ValidateSummary))]
        [Tooltip("This will be placed in the build folder's name")]
        [System.NonSerialized, ShowInInspector]
        public string Summary;

        [TextArea(1, 10)]
        [Tooltip("This will be placed as a CHANGELOG.MD file inside the built folder")]
        [System.NonSerialized, ShowInInspector]
        public string Changelog;

        private bool ValidateSummary(string value, ref string message, ref InfoMessageType? messageType)
        {
            if (!string.IsNullOrEmpty(value) && value.Length > 30)
            {
                message = "Summary should be shorter!!!";
                messageType = InfoMessageType.Warning;
                return false;
            }

            return true;
        }
    }

    [System.Serializable]
    public class BuildStandaloneSettings : BuildPlatformSettings
    {
        public override BuildTarget Target => BuildTarget.StandaloneWindows64;
        public override BuildTargetGroup TargetGroup => BuildTargetGroup.Standalone;

        public string PlayerExectable = "exe";
        public bool WaitForManagedDebugger = false;
    }

    [System.Serializable]
    public class BuildAndroidSettings /*: BuildPlatformSettings*/
    {
        //public override BuildTarget Target => BuildTarget.Android;
        //public override BuildTargetGroup TargetGroup => BuildTargetGroup.Android;

        [Button("Open Build Settings", ButtonSizes.Large)]
        [PropertyOrder(102)]
        private void OpenBuildSettings()
        {
            // Open BuildSettings as a dockable window
            EditorWindow.GetWindow(System.Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));
        }
    }

    [System.Serializable]
    public class BuildWebGLSettings : BuildPlatformSettings
    {
        public override BuildTarget Target => BuildTarget.WebGL;
        public override BuildTargetGroup TargetGroup => BuildTargetGroup.WebGL;
    }
}
