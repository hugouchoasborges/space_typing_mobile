using UnityEngine;

namespace myproject.player
{
    public class PlayerController : MonoBehaviour
    {
        public void Move(Vector2 delta)
        {
            transform.position += (Vector3)delta;
        }

        public void Shoot()
        {
            // MEDO: Create projectile

        }
    }
}