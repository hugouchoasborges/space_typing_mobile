using System;
using UnityEngine;

namespace tools.attributes
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public class HorizontalLineAttribute : PropertyAttribute
    {
        public const float DEFAULT_HEIGHT = 2.0f;

        public float Height { get; private set; }
        public Color Color { get; private set; }

        public HorizontalLineAttribute(float height) : this(height, 0, 0, 0) { }
        public HorizontalLineAttribute(float height, float r, float g, float b)
        {
            Height = height;
            Color = new Color(r, g, b);
        }
    }
}
