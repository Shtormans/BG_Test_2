using Fusion;
using UnityEngine;

namespace MainGame
{
    public class RifleBullet : Bullet
    {
        [Networked] private TickTimer life { get; set; }
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public override void StartMoving(float speed, float secondsToDisappear)
        {
            life = TickTimer.CreateFromSeconds(Runner, secondsToDisappear);
            _rigidbody.velocity = transform.right * speed;
        }

        public override void FixedUpdateNetwork()
        {
            if (life.Expired(Runner))
                Runner.Despawn(Object);
        }
    }
}
