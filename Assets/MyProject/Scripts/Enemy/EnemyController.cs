using UnityEngine;

namespace enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyController : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        //private void Start()
        //{
        //    // MEDO: Remove mocked startup
        //    // Mock Startup
        //    SetImpulse(Vector2.down * 5);
        //}

        public void SetImpulse(Vector2 direction)
        {
            _rigidbody2D.AddForce(direction, ForceMode2D.Impulse);
        }
    }
}
