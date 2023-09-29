using UnityEngine;

namespace tools.attributes
{
    public class OnValidateButtonAttribute : PropertyAttribute
    {
        public string ButtonLabel;

        public OnValidateButtonAttribute(string buttonLabel)
        {
            ButtonLabel = buttonLabel;
        }

        private OnValidateButtonAttribute()
        {
            ButtonLabel = "Button";
        }
    }
}
