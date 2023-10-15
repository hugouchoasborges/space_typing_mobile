using UnityEditor;
using UnityEngine;

namespace tools.attributes
{
    [CustomPropertyDrawer(typeof(OnValidateButtonAttribute))]
    public class OnValidateButtonDrawer : PropertyDrawer
    {
        private OnValidateButtonAttribute _attribute = null;
        public OnValidateButtonAttribute Attribute
        {
            get
            {
                if (_attribute == null)
                    _attribute = (OnValidateButtonAttribute)attribute;

                return _attribute;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (GUI.Button(position, new GUIContent(Attribute.ButtonLabel)))
            {
                property.boolValue = true;
            }
        }
    }
}
