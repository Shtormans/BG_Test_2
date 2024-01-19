using Fusion;
using System;
using UnityEngine;

namespace MainGame
{
    public abstract class Bullet : NetworkBehaviour
    {
        public event Action<Entity> Hit;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Entity entity))
            {
                Hit?.Invoke(entity);
            }
        }

        public abstract void StartMoving(float speed, float secondsToDisappear);
    }
}
