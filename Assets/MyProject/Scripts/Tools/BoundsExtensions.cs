using UnityEngine;

namespace tools
{
    public static class BoundsExtensions
    {
        public static Vector3[] GetCorners(this Bounds bounds)
        {
            Vector3[] corners = new Vector3[4];

            corners[0] = bounds.min;
            corners[1] = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
            corners[2] = bounds.max;
            corners[3] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);

            return corners;
        }
    }
}
