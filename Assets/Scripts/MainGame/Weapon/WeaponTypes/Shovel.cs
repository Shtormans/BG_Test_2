using System.Collections;
using UnityEngine;

namespace MainGame
{
    public class Shovel : Weapon
    {
        [SerializeField] private Animator _animator;
        private BoxCollider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
        }

        private void OnEnable()
        {
            Init();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent<Entity>(out var entity) && entity is PlayerBehaviour)
            {
                OnHit(entity);
            }
        }

        protected override void OnHit(Entity entity)
        {
            var hitStatus = entity.TakeDamage((uint)Stats.Damage);
            TriggerHitEvent(hitStatus);
        }

        protected override void Fire()
        {
            _collider.enabled = true;
            _animator.SetTrigger(AnimationTriggers.Hit.ToString());
        }

        private IEnumerator AwaitHit()
        {
            yield return new WaitForSeconds(Stats.FireRate);

            _collider.enabled = false;
        }
    }
}
