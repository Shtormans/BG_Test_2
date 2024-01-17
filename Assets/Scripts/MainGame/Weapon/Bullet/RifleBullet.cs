using UnityEngine;

namespace MainGame
{
    public class RifleBullet : Bullet
    {
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public override void StartMoving(float speed, float secondsToDisappear)
        {
            _rigidbody.velocity = transform.forward * speed;

            Destroy(gameObject, secondsToDisappear);
        }
    }
}
