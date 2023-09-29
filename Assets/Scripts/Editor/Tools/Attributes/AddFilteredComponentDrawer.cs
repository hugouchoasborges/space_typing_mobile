using tools.advanceddropdowns;
using System;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

using Object = UnityEngine.Object;

namespace tools.attributes
{
    [CustomPropertyDrawer(typeof(AddFilteredComponentAttribute))]
    public class AddFilteredComponentDrawer : PropertyDrawer
    {

        private AdvancedDropdownState _dropdownState;

        private AddFilteredComponentAttribute _attribute = null;
        public AddFilteredComponentAttribute Attribute
        {
            get
            {
                if (_attribute == null)
                    _attribute = (AddFilteredComponentAttribute)attribute;

                return _attribute;
            }
        }

        protected Type attributeType = null;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        private Object _selectedObject;
        protected Object selectedObject
        {
            set
            {
                if (_selectedObject != value)
                {
                    _selectedObject = value;
                }
            }
            get
            {
                return _selectedObject;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // If property is filled
            if (property.objectReferenceValue != null)
            {
                Rect leftRect = new Rect(position.x, position.y, position.width * .75f, position.height);
                Rect rightRect = new Rect(leftRect.x + leftRect.width, leftRect.y, position.width - leftRect.width, position.height);

                // Just show it -- READ ONLY
                GUI.enabled = false;
                EditorGUI.PropertyField(leftRect, property, label, true);
                GUI.enabled = true;

                // Button to remove it
                if (GUI.Button(rightRect, new GUIContent("Clear"), EditorStyles.miniButton))
                {
                    OnRemoveItemSelected(property);
                }

                return;
            }

            if (attributeType == null)
                attributeType = Attribute.Type != null ? Attribute.Type : fieldInfo.FieldType;

            Rect leftListRect = new Rect(position.x + 15, position.y, position.width * .35f, position.height);
            Rect rightListRect = new Rect(leftListRect.x + leftListRect.width, leftListRect.y, position.width - leftListRect.width, position.height);

            GUI.Label(leftListRect, label);

            if (GUI.Button(rightListRect, new GUIContent(Attribute.ButtonLabel), EditorStyles.miniButton))
            {
                if (_dropdownState == null)
                    _dropdownState = new AdvancedDropdownState();

                GenericClassDropdown dropdown = new GenericClassDropdown(_dropdownState, attributeType, (item) => OnItemSelected(item, property), Attribute.OnlyInCurrentGameObject ? ((MonoBehaviour)property.serializedObject.targetObject).gameObject : null);

                dropdown.Show(position);
            }
        }

        private void OnItemSelected(GenericClassDropdownItem item, SerializedProperty property)
        {
            // Add a new component
            Component component = ((MonoBehaviour)property.serializedObject.targetObject).gameObject.GetComponent(item.ClassType);
            if (component == null)
                component = ((MonoBehaviour)property.serializedObject.targetObject).gameObject.AddComponent(item.ClassType);

            // Link the added component to the field
            property.serializedObject.Update();
            property.objectReferenceValue = component;
            property.serializedObject.ApplyModifiedProperties();
        }

        private void OnRemoveItemSelected(SerializedProperty property)
        {
            // Link the added component to the field
            property.serializedObject.Update();
            property.objectReferenceValue = null;
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}
