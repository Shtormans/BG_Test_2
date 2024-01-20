using Fusion;
using System;
using UnityEngine;

namespace MainGame
{
    public abstract class Bullet : NetworkBehaviour
    {
        public event Action<Entity> Hit;
        private bool _hit = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_hit)
            {
                return;
            }

            if (collision.gameObject.TryGetComponent(out Entity entity))
            {
                Hit?.Invoke(entity);
                _hit = true;

                if (HasStateAuthority)
                {
                    Runner.Despawn(Object);
                }
            }
        }

        public abstract void StartMoving(float speed, float secondsToDisappear);
    }
}
