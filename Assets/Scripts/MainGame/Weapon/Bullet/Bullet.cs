using System;
using UnityEngine;

namespace MainGame
{
    public abstract class Bullet : MonoBehaviour
    {
        public event Action<Entity> Hit;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Entity entity))
            {
                Hit?.Invoke(entity);
            }
        }

        public abstract void StartMoving(float speed, float secondsToDisappear);
    }
}
