using UnityEngine;

namespace tools
{
    public static class Vector2Extensions
    {

        public static float GetRandomFloat(this Vector2 vec2)
        {
            return Random.Range(vec2.x, vec2.y);
        }
    }
}
