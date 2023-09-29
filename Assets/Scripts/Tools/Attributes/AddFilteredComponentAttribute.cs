using System;
using UnityEngine;

namespace tools.attributes
{
    public class AddFilteredComponentAttribute : PropertyAttribute
    {
        public readonly string ButtonLabel;
        public readonly bool OnlyInCurrentGameObject;
        public readonly Type Type;

        public AddFilteredComponentAttribute(
            string buttonLabel = "Select",
            bool onlyInCurrentGameObject = false,
            Type filterType = null
            )
        {
            ButtonLabel = buttonLabel;
            OnlyInCurrentGameObject = onlyInCurrentGameObject;
            Type = filterType;
        }
    }
}
