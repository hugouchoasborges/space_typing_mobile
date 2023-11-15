using Sirenix.OdinInspector;
using tools.attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace tools
{
    public class CustomEnumSO : ScriptableObject
    {
        [LabelText("Default Entry"), ShowInInspector, PropertyOrder(-100)] public virtual string DEFAULT_ENTRY => "NONE";
        [LabelText("Default Additional Entries"), ShowInInspector, PropertyOrder(-100), ListDrawerSettings(Expanded = true, ShowIndexLabels = false, ShowItemCount = false, DraggableItems = true, NumberOfItemsPerPage = 6)]
        public virtual string[] DEFAULT_ADDITIONAL_ENTRIES => null;
        protected virtual string ENTRIES_TITLE => "ENTRIES";
        public virtual string ENUM_ENTRIES_REGEX => @"\senum\s[\s\w\d\n\r]*\{([\s\S]*?)\}";

        [LabelText("$ENTRIES_TITLE")]
        [SerializeField, RuntimeReadOnly, ListDrawerSettings(Expanded = true, ShowIndexLabels = false, ShowItemCount = false, DraggableItems = true, NumberOfItemsPerPage = 6)]
        protected List<string> enumTypes;

        [SerializeField, ListDrawerSettings(Expanded = false, ShowIndexLabels = false, ShowItemCount = false, DraggableItems = true, NumberOfItemsPerPage = 6)]
        protected List<CachedEnumEntry> cachedEnumTypes = new List<CachedEnumEntry>();

#if UNITY_EDITOR

        [HideInNormalInspector(true)]
        [InfoBox("Script can't be null.", InfoMessageType.Error, VisibleIf = "EnumScriptNull")]
        public MonoScript EnumScript;

        private bool EnumScriptNull() => EnumScript == null;

        protected virtual void EnsureMinimumEventTypes()
        {
            // Ensure the Default entry is set
            if (!string.IsNullOrEmpty(DEFAULT_ENTRY))
            {
                AddEnumType(DEFAULT_ENTRY);
            }

            // Ensure the default additional entries are set
            if (DEFAULT_ADDITIONAL_ENTRIES != null)
                foreach (var additionalEntry in DEFAULT_ADDITIONAL_ENTRIES)
                    AddEnumType(additionalEntry, -1);
        }

        /// <summary>
        /// Adds an entry to the enumTypes list
        /// </summary>
        /// <param name="entry">The entry to be added</param>
        /// <param name="index">Position to be added. (-1) will insert it at the last position</param>
        private void AddEnumType(string entry, int index = -1)
        {
            if (enumTypes == null)
                enumTypes = new List<string>();

            if (index < 0)
                index = enumTypes.Count;

            if (!enumTypes.Contains(entry))
                enumTypes.Insert(index, entry);
        }

        protected string[] GetEnumSplitEntriesNames()
        {
            string text = GetEnumEntriesNames();
            if (string.IsNullOrWhiteSpace(text)) return null;

            string[] entries = text.Replace(" ", "").Split(',');
            return entries.Where(e => !string.IsNullOrWhiteSpace(e)).ToArray();
        }

        protected string GetEnumEntriesNames()
        {
            Match match = Regex.Match(EnumScript.text, ENUM_ENTRIES_REGEX);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return null;
        }

        private void UpdateCachedEntries(string[] entries)
        {
            // Look for duplicated counts
            List<int> duplicatedCountCheck = new List<int>();
            foreach (var cachedEnum in cachedEnumTypes)
            {
                if (duplicatedCountCheck.Contains(cachedEnum.Count))
                {
                    // Throw Exception
                    throw new System.Exception(string.Format("Found a duplicated enum: {0}" +
                        "\nFIX IT IMMEDIATELY", cachedEnum));
                }
                duplicatedCountCheck.Add(cachedEnum.Count);
            }

            // Retrieve the next count
            int nextCount = -1;
            foreach (var cachedEnum in cachedEnumTypes)
                if (cachedEnum.Count > 0)
                    nextCount = cachedEnum.Count;

            nextCount++;

            for (int i = 0; i < enumTypes.Count; i++)
            {
                string enumType = enumTypes[i];
                bool found = false;
                foreach (var cachedEnumType in cachedEnumTypes)
                {
                    if (cachedEnumType.Name == enumType)
                    {
                        // Mark this entry as enabled
                        cachedEnumType.Enabled = true;

                        found = true;
                        break;
                    }
                }

                // Entry found -- Ignore it
                if (found)
                    continue;

                // Entry not found -- Cache it 
                cachedEnumTypes.Add(new CachedEnumEntry(enumType, nextCount++));
            }

            // Now Disable all cached elements that aren't in the serialized list anymore
            foreach (var cachedEnumType in cachedEnumTypes)
            {
                bool found = false;
                foreach (var enumType in enumTypes)
                {
                    if (enumType == cachedEnumType.Name)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    // Mark this entry as disabled
                    cachedEnumType.Enabled = false;
                }
            }

            // Now reorder the cached enum list to have the same sequence as the serialized one
            cachedEnumTypes.Sort((a, b) =>
            {
                if (enumTypes.Contains(a.Name) && !enumTypes.Contains(b.Name))
                    return -1;

                if (!enumTypes.Contains(a.Name) && enumTypes.Contains(b.Name))
                    return -1;

                if (!enumTypes.Contains(a.Name) && !enumTypes.Contains(b.Name))
                    return 0;

                return enumTypes.IndexOf(a.Name).CompareTo(enumTypes.IndexOf(b.Name));
            });
        }

        [PropertySpace(SpaceBefore = 10, SpaceAfter = 0)]
        [Button("Apply Changes")]
        public virtual void ApplyChanges()
        {
            if (EnumScript == null) throw new NullReferenceException("Selected Script cannot be null. Make sure you've selected a script");

            // Remove numerical and empty entries
            for (int i = enumTypes.Count - 1; i >= 0; i--)
            {
                if (string.IsNullOrWhiteSpace(enumTypes[i]))
                    enumTypes.RemoveAt(i);

                else if (char.IsDigit(enumTypes[i][0]))
                    enumTypes.RemoveAt(i);
            }

            // Remove duplicated entries
            enumTypes = enumTypes.Distinct().ToList();

            // Update the Cached entries
            UpdateCachedEntries(enumTypes.Distinct().ToArray());

            try
            {
                AssetDatabase.StartAssetEditing();

                string enumsStr = "\n";
                foreach (var eventType in cachedEnumTypes)
                {
                    enumsStr += string.Format("\t\t{0},\n", eventType);
                }

                string currentEntries = GetEnumEntriesNames();
                if (!string.IsNullOrWhiteSpace(currentEntries))
                {
                    string replacedScript = Regex.Replace(EnumScript.text, currentEntries, enumsStr);
                    File.WriteAllText(AssetDatabase.GetAssetPath(EnumScript), replacedScript);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
                EditorUtility.SetDirty(EnumScript);
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
            }

            EditorApplication.delayCall += OnEnumChanged_NextFrame;
        }

        [Button("Revert Changes")]
        public virtual void RevertChanges()
        {
            if (EnumScript == null) throw new NullReferenceException("Selected Script cannot be null. Make sure you've selected a script");

            string[] currentEntries = GetEnumSplitEntriesNames();
            if (currentEntries != null)
            {
                enumTypes = currentEntries.ToList();
            }

            EditorApplication.delayCall += OnEnumChanged_NextFrame;
        }

        protected virtual void OnEnumChanged_NextFrame() { }

        public virtual bool HasUnsavedChanges()
        {
            // Get current on disk entries
            string[] currentDiskEntries = GetEnumSplitEntriesNames();

            // Get current on memory (inspector) entries
            string[] currentInspectorEntries = enumTypes.ToArray();

            // Compare them
            return !Enumerable.SequenceEqual(currentDiskEntries, currentInspectorEntries);
        }

        protected virtual void OnValidate()
        {
            EnsureMinimumEventTypes();
        }
#endif
    }

    [System.Serializable]
    public class CachedEnumEntry
    {
        public string Name;
        public int Count;
        public bool Enabled;

        public CachedEnumEntry(string name, int count)
        {
            Name = name;
            Count = count;
            Enabled = true;
        }

        public override string ToString() => string.Format("{0} = {1}", Name, Count);
    }
}
