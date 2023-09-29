using UnityEditor;
using UnityEngine;

namespace tools.attributes
{
    [CustomPropertyDrawer(typeof(HorizontalLineAttribute))]
    class HorizontalLineDrawer : DecoratorDrawer
    {
        private HorizontalLineAttribute _lineAttr => (HorizontalLineAttribute)attribute;
        public override float GetHeight()
        {
            return EditorGUIUtility.singleLineHeight + _lineAttr.Height;
        }

        public override void OnGUI(Rect position)
        {
            Rect rect = EditorGUI.IndentedRect(position);
            rect.y += EditorGUIUtility.singleLineHeight / 3f;

            // Draw the line
            rect.height = _lineAttr.Height;
            EditorGUI.DrawRect(rect, _lineAttr.Color);
        }
    }
}
