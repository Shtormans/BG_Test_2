using System.Collections;
using UnityEngine;

namespace MainGame
{
    public class Shovel : Weapon
    {
        private BoxCollider2D _collider;
        private WeaponAnimationController _animationController;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
            _animationController = GetComponent<WeaponAnimationController>();
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
            if (_collider.enabled == true)
            {
                return;
            }

            _collider.enabled = true;
            _animationController.SetAnimationTrigger(AnimationTriggers.Hit);

            StartCoroutine(AwaitHit());
        }

        private IEnumerator AwaitHit()
        {
            yield return new WaitForSeconds(Stats.FireRate);

            _collider.enabled = false;
        }
    }
}
