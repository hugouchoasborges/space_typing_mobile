using Sirenix.OdinInspector;
using tools;
using tools.attributes;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace scenes.settings
{
    public class SceneHelperTypeSettingsSO : CustomEnumSO
    {
        public static SceneHelperTypeSettingsSO Instance => MenuExtensions.LoadSOFromResources<SceneHelperTypeSettingsSO>(Path.Combine(SceneHelperSettingsSO.CONFIG_FILE_ROOT, SceneHelperSettingsSO.CONFIG_FILE_TYPES));

        [LabelText("Scenes Settings")]
        [PropertySpace(SpaceBefore = 10, SpaceAfter = 0)]
        [ListDrawerSettings(Expanded = true, ShowIndexLabels = false, ShowItemCount = false, DraggableItems = false, HideRemoveButton = true, HideAddButton = true)]
        [UnityEngine.SerializeField]
        private List<SceneEntrySettingsItem> _entries;

        public string GetScenePath(SceneType sceneType)
        {
            return GetScenePath(sceneType.ToString());
        }

        public string GetScenePath(string sceneType)
        {
            foreach (var entry in _entries)
                if (entry.SceneType == sceneType)
                    return entry.Scene;

            return "";
        }

#if UNITY_EDITOR
        [Button("Scene Settings")]
        private void GoToFSMSettings()
        {
            SceneHelperSettingsSO.MenuItem_SceneSettings();
        }

        protected override void OnEnumChanged_NextFrame()
        {
            AssetDatabase.StartAssetEditing();

            List<SceneEntrySettingsItem> newEntries = new List<SceneEntrySettingsItem>();

            foreach (var enumType in enumTypes)
                newEntries.Add(new SceneEntrySettingsItem(enumType, GetScenePath(enumType)));

            _entries.Clear();
            _entries = newEntries;

            AssetDatabase.StopAssetEditing();
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }
#endif
    }

    [System.Serializable]
    public class SceneEntrySettingsItem
    {
        [ReadOnly]
        [HorizontalGroup("Group")]
        [HideLabel]
        public string SceneType;

        [Scene]
        [HorizontalGroup("Group")]
        [HideLabel]
        public string Scene;

        public SceneEntrySettingsItem(string sceneType, string scene)
        {
            SceneType = sceneType;
            Scene = scene;
        }
    }
}
