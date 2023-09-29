using log;
using System;
using System.IO;
using UnityEditor;

namespace build
{
    public class BuildUtils
    {
        public static void Build(BuildPlatformSettings settings)
        {
            DoBuild(settings);
        }

        private static void DoBuild(BuildPlatformSettings settings)
        {
            try
            {
                string targetRootPath = BuildSettingsSO.GetBuildPath(settings);
                string targetFullPath = targetRootPath;

                if (settings is BuildStandaloneSettings windowsSettings)
                {
                    // Windows specific
                    targetFullPath = Path.Combine(targetRootPath, windowsSettings.PlayerExectable);
                    EditorUserBuildSettings.waitForManagedDebugger = windowsSettings.WaitForManagedDebugger;
                }
                //else if (settings is BuildAndroidSettings androidSettings)
                //{
                //    // Android specific
                //}
                else if (settings is BuildWebGLSettings webglSettings)
                {
                    // WebGL specific
                }


                ELog.Log_Debug(ELogType.BUILD, "Build Started. \nTarget: {0}", targetFullPath);

                EditorUserBuildSettings.SetBuildLocation(settings.Target, targetRootPath);
                var report = BuildPipeline.BuildPlayer(BuildSettingsSO.Instance.BuildScenes, targetFullPath, settings.Target, settings.BuildOptions);

                if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
                {
                    CreateChangelogFile(targetRootPath, settings.Notes.Changelog);
                    ELog.Log_Debug(ELogType.BUILD, "Build Finished Successfully");
                }
                else
                {
                    ELog.Log_Debug(ELogType.BUILD, "Build Failed");
                }
            }
            catch (Exception e)
            {
                ELog.LogError_Debug(ELogType.BUILD, "Build Failed");
                throw e;
            }
            finally
            {

            }
        }

        private static void CreateChangelogFile(string path, string changelog)
        {
            string filePath = Path.Combine(path, "CHANGELOG.md");

            ELog.Log_Debug(ELogType.BUILD, "Creating CHANGELOG file... {0}", filePath);

            if (File.Exists(filePath))
                File.Delete(filePath);

            using (var fs = new FileStream(filePath, FileMode.Append))
            using (var sw = new StreamWriter(fs))
            {
                sw.WriteLine("# CHANGELOG");
                sw.WriteLine(changelog);
            }
        }
    }
}
