using System;
using UnityEditor;
using UnityEngine;

namespace tools
{
    /// <summary>
    /// Tutorial -- https://catlikecoding.com/unity/tutorials/editor/custom-list/
    /// </summary>
    public static class InspectorList
    {
        private static bool _foldout = false;
        public static void Show(SerializedProperty list, InspectorListOptions options = InspectorListOptions.Default, string customLabel = "")
        {
            if (!list.isArray)
            {
                EditorGUILayout.HelpBox(list.name + " is neither an array nor a list!", MessageType.Error);
                return;
            }

            bool
                showListLabel = (options & InspectorListOptions.Label) != 0,
                showFoldout = (options & InspectorListOptions.Foldout) != 0,
                showListSize = (options & InspectorListOptions.Size) != 0;

            if (showListLabel)
            {
                string label = !string.IsNullOrEmpty(customLabel) ? customLabel : list.displayName;
                if (showFoldout)
                    _foldout = EditorGUILayout.Foldout(_foldout, label, true);
                else
                    EditorGUILayout.LabelField(label, EditorStyles.boldLabel);

                EditorGUI.indentLevel += 1;
            }

            if (!showFoldout || (showFoldout && _foldout))
            {
                if (showListSize)
                    EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));

                ShowElements(list, options);
            }

            if (showListLabel)
                EditorGUI.indentLevel -= 1;
        }

        private static void ShowElements(SerializedProperty list, InspectorListOptions options)
        {
            bool
                showElementsLabel = (options & InspectorListOptions.ElementsLabel) != 0,
                showElementsBox = (options & InspectorListOptions.ElementsBox) != 0;

            if (showElementsBox)
                GUILayout.BeginVertical("BOX");

            for (int i = 0; i < list.arraySize; i++)
            {
                SerializedProperty entry = list.GetArrayElementAtIndex(i);
                GUIContent label = !showElementsLabel ? GUIContent.none : new GUIContent(entry.displayName);

                EditorGUILayout.PropertyField(entry, label);
            }

            if (showElementsBox)
                GUILayout.EndVertical();
        }
    }

    [Flags]
    public enum InspectorListOptions
    {
        None = 0,

        Label = 1,
        Foldout = 2,
        Size = 4,
        ElementsLabel = 8,
        ElementsBox = 16,

        Default = Label | ElementsLabel | Foldout | ElementsBox
    }
}
