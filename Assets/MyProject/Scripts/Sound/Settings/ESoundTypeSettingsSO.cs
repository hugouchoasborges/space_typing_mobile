using Sirenix.OdinInspector;
using tools;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace sound.settings
{
    public class ESoundTypeSettingsSO : CustomEnumSO
    {
        public static ESoundTypeSettingsSO Instance => MenuExtensions.LoadSOFromResources<ESoundTypeSettingsSO>(Path.Combine(ESoundSettingsSO.CONFIG_FILE_ROOT, ESoundSettingsSO.CONFIG_FILE_TYPES));

        [LabelText("Sound Type Settings")]
        [PropertySpace(SpaceBefore = 10, SpaceAfter = 0)]
        [ListDrawerSettings(Expanded = true, ShowIndexLabels = false, ShowItemCount = false, DraggableItems = false, HideRemoveButton = true, HideAddButton = true, NumberOfItemsPerPage = 6)]
        [UnityEngine.SerializeField]
        private List<SoundEntrySettingsItem> _entries;


        Dictionary<ESoundType, AudioClip> _soundMap = null;
        public Dictionary<ESoundType, AudioClip> GetSoundMap()
        {
            if (_soundMap == null)
            {
                _soundMap = new Dictionary<ESoundType, AudioClip>();

                // Get all values of the DaysOfWeek enum
                ESoundType[] soundTypes = (ESoundType[])Enum.GetValues(typeof(ESoundType));

                // Iterate through each enum entry
                foreach (ESoundType soundType in soundTypes)
                {
                    _soundMap.Add(soundType, GetAudioClip(soundType));
                }
            }

            return _soundMap;
        }

        public AudioClip GetAudioClip(ESoundType soundType) => GetAudioClip(soundType.ToString());

        public AudioClip GetAudioClip(string soundType)
        {
            foreach (var entry in _entries)
                if (entry.SoundType == soundType)
                    return entry.AudioClip;

            return null;
        }

#if UNITY_EDITOR
        [Button("SOUND Settings")]
        private void GoToFSMSettings()
        {
            ESoundSettingsSO.MenuItem_SoundSettings();
        }

        protected override void OnEnumChanged_NextFrame()
        {
            AssetDatabase.StartAssetEditing();

            List<SoundEntrySettingsItem> newEntries = new List<SoundEntrySettingsItem>();

            foreach (var enumType in enumTypes)
                newEntries.Add(new SoundEntrySettingsItem(enumType, GetAudioClip(enumType)));

            _entries.Clear();
            _entries = newEntries;

            AssetDatabase.StopAssetEditing();
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }
#endif
    }

    [System.Serializable]
    public class SoundEntrySettingsItem
    {
        [ReadOnly]
        [HorizontalGroup("Group")]
        [HideLabel]
        public string SoundType;

        [HorizontalGroup("Group")]
        [HideLabel]
        public AudioClip AudioClip;

        public SoundEntrySettingsItem(string soundType, AudioClip audioClip)
        {
            SoundType = soundType;
            AudioClip = audioClip;
        }
    }
}
