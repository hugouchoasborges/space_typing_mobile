using tools;
using UnityEngine;

namespace background
{
    public class BackgroundController : MonoBehaviour
    {
        [Header("Stars")]
        [SerializeField]
        private Bounds _starsBounds =
            new Bounds(Vector3.zero, new Vector3(17 * 2, 29 * 2, 0));
        [SerializeField] private ParticleSystem _starsParticleSystem;

        [Header("Meteors")]
        [SerializeField]
        private Bounds _meteorsBounds =
            new Bounds(Vector3.zero, new Vector3(17 * 2, 29 * 2, 0));
        [SerializeField] private ParticleSystem _meteorsParticleSystem;


        // ----------------------------------------------------------------------------------
        // ========================== Handling Stars ============================
        // ----------------------------------------------------------------------------------

        public void PlayStars()
        {
            _starsParticleSystem.Play();
        }

        public void StopStars()
        {
            _starsParticleSystem.Stop();
        }

        private void UpdateStarsBounds()
        {
            var shapeModule = _starsParticleSystem.shape;
            shapeModule.scale = _starsBounds.size;
        }


        // ----------------------------------------------------------------------------------
        // ========================== Handling Meteors ============================
        // ----------------------------------------------------------------------------------

        public void PlayMeteors()
        {
            _meteorsParticleSystem.Play();
        }
        public void StopMeteors()
        {
            _meteorsParticleSystem.Stop();
        }

        private void UpdateMeteorsBounds()
        {
            var shapeModule = _meteorsParticleSystem.shape;
            shapeModule.scale = _meteorsBounds.size;
        }

        // ----------------------------------------------------------------------------------
        // ========================== Gizmos\Editor Stuff ============================
        // ----------------------------------------------------------------------------------

#if UNITY_EDITOR


        void OnDrawGizmosSelected()
        {
            // Draw stars
            GizmosExtensions.DrawBounds(_starsBounds, Color.yellow);
            GizmosExtensions.DrawBounds(_meteorsBounds, Color.blue);
        }

        private void OnValidate()
        {
            UpdateStarsBounds();
            UpdateMeteorsBounds();
        }
#endif
    }
}
