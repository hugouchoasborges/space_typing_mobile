using DG.Tweening.Core.Easing;
using tools;
using UnityEngine;

namespace utils
{
    public class ScreenBoundaries : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool _lockMovement = true;
        [SerializeField] private Bounds _screenBounds;

        private void LateUpdate()
        {
            if (!_lockMovement) return;

            // Limit the player's movement within the screen boundaries
            Vector3 clampedPosition = transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, _screenBounds.min.x, _screenBounds.max.x);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, _screenBounds.min.y, _screenBounds.max.y);

            transform.position = clampedPosition;
        }


        // ----------------------------------------------------------------------------------
        // ========================== Gizmos Stuff ============================
        // ----------------------------------------------------------------------------------


#if UNITY_EDITOR

        void OnDrawGizmosSelected()
        {
            GizmosExtensions.DrawBounds(_screenBounds, Color.yellow);
        }
#endif
    }
}
