using Sirenix.OdinInspector;
using tools;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace log.settings
{
    public class ELogTypeSettingsSO : CustomEnumSO
    {
        public static ELogTypeSettingsSO Instance => MenuExtensions.LoadSOFromResources<ELogTypeSettingsSO>(Path.Combine(ELogSettingsSO.CONFIG_FILE_ROOT, ELogSettingsSO.CONFIG_FILE_TYPES));

        [LabelText("Log Type Settings")]
        [PropertySpace(SpaceBefore = 10, SpaceAfter = 0)]
        [ListDrawerSettings(Expanded = true, ShowIndexLabels = false, ShowItemCount = false, DraggableItems = false, HideRemoveButton = true, HideAddButton = true, NumberOfItemsPerPage = 6)]
        [UnityEngine.SerializeField]
        private List<LogEntrySettingsItem> _entries;

        public bool EntryEnabled(ELogType logType)
        {
            return EntryEnabled(logType.ToString());
        }

        public bool EntryEnabled(string logType)
        {
            foreach (var entry in _entries)
                if (entry.LogType == logType)
                    return entry.Enabled;

            return false;
        }

#if UNITY_EDITOR
        [Button("LOG Settings")]
        private void GoToFSMSettings()
        {
            ELogSettingsSO.MenuItem_LogSettings();
        }

        protected override void OnEnumChanged_NextFrame()
        {
            AssetDatabase.StartAssetEditing();

            List<LogEntrySettingsItem> newEntries = new List<LogEntrySettingsItem>();

            foreach (var enumType in enumTypes)
                newEntries.Add(new LogEntrySettingsItem(enumType, EntryEnabled(enumType)));

            _entries.Clear();
            _entries = newEntries;

            AssetDatabase.StopAssetEditing();
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }
#endif
    }

    [System.Serializable]
    public class LogEntrySettingsItem
    {
        [ReadOnly]
        [HorizontalGroup("Group")]
        [HideLabel]
        public string LogType;

        [HorizontalGroup("Group")]
        [HideLabel]
        public bool Enabled;

        public LogEntrySettingsItem(string logType, bool enabled)
        {
            LogType = logType;
            Enabled = enabled;
        }
    }
}
