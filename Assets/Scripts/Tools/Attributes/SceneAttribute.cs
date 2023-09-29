using UnityEngine;

namespace tools.attributes
{
    public class SceneAttribute : PropertyAttribute
    {
        public bool AllowOutsideBuildReference = true;

        public SceneAttribute(bool allowOutsideBuildReference) : this()
        {
            AllowOutsideBuildReference = allowOutsideBuildReference;
        }

        public SceneAttribute() { }
    }
}
