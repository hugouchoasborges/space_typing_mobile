using UnityEngine;

namespace tools
{
    public static class GizmosExtensions
    {
        public static void DrawBounds(Bounds bounds, Color? color = null)
        {
            if (color.HasValue)
                Gizmos.color = color.Value;

            Vector3[] corners = bounds.GetCorners();

            // Draw the wireframe using the corner points
            for (int i = 0; i < corners.Length; i++)
                Gizmos.DrawLine(corners[i], corners[(i + 1) % 4]);
        }
    }
}
