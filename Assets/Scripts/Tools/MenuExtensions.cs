using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace tools
{
    public class MenuExtensions
    {
        public static T LoadSOFromResources<T>(string path) where T : ScriptableObject
        {
            string resourcesPath = path.Contains("Resources/") ? path.Substring(path.IndexOf("Resources/")).Replace("Resources/", "").Replace(".asset", "") : path;
            T loaded = Resources.Load<T>(resourcesPath);
#if UNITY_EDITOR
            if (loaded == null && !EditorApplication.isPlayingOrWillChangePlaymode)
                loaded = PingOrCreateSO<T>(path);
#endif
            if (loaded == null)
                throw new System.NullReferenceException("Resource not found: " + resourcesPath);

            return loaded;
        }

#if UNITY_EDITOR
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns>The selected\created asset</returns>
        public static T PingOrCreateSO<T>(string path, bool newWindow = true, Vector2 minSize = default(Vector2), Vector2 maxSize = default(Vector2)) where T : ScriptableObject
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();

                var rootPath = Directory.GetParent(path);
                if (!rootPath.Exists)
                    rootPath.Create();

                AssetDatabase.CreateAsset(asset, path);
            }

            EditorGUIUtility.PingObject(asset);
            if (newWindow)
            {
                // Open asset properties on a new window
                PopupAssetInspector.Create(asset, minSize, maxSize);

            }
            else
            {
                // Select the asset and display it in the existing inspector
                Selection.activeObject = asset;
            }

            return asset;
        }
#endif
    }

#if UNITY_EDITOR
    public class PopupAssetInspector : EditorWindow
    {
        private Object asset;
        private Editor assetEditor;

        public static PopupAssetInspector Create(Object asset, Vector2 minSize = default(Vector2), Vector2 maxSize = default(Vector2))
        {
            var window = CreateWindow<PopupAssetInspector>($"{asset.name} | {asset.GetType().Name}");
            window.asset = asset;
            window.assetEditor = Editor.CreateEditor(asset);

            if (minSize != default(Vector2)) window.minSize = minSize;
            if (maxSize != default(Vector2)) window.maxSize = maxSize;

            return window;
        }

        private void OnGUI()
        {
            GUI.enabled = false;
            asset = EditorGUILayout.ObjectField("Asset", asset, asset.GetType(), false);
            GUI.enabled = true;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            assetEditor.OnInspectorGUI();
            EditorGUILayout.EndVertical();
        }
    }
#endif
}
