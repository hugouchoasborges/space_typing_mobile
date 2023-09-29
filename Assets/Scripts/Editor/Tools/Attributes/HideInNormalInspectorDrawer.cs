using UnityEditor;
using UnityEngine;

namespace tools.attributes
{
    [CustomPropertyDrawer(typeof(HideInNormalInspectorAttribute))]
    class HideInNormalInspectorDrawer : PropertyDrawer
    {
        private HideInNormalInspectorAttribute _attribute => (HideInNormalInspectorAttribute)attribute;

        private bool Visible(SerializedProperty property) => 
            _attribute != null && _attribute.ShowWhenNull 
            && property.propertyType == SerializedPropertyType.ObjectReference 
            && property.objectReferenceValue == null;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (Visible(property))
                return EditorGUI.GetPropertyHeight(property, label, true);

            return 0f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (Visible(property))
                EditorGUI.PropertyField(position, property, label);

        }
    }
}
