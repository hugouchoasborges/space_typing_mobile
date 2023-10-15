using UnityEngine;

namespace tools.attributes
{
    public class HideInNormalInspectorAttribute : PropertyAttribute
    {
        public bool ShowWhenNull;

        public HideInNormalInspectorAttribute(bool showWhenNull) : base()
        {
            ShowWhenNull = showWhenNull;
        }

        public HideInNormalInspectorAttribute() : base() { }
    }
}
