using UnityEngine;

namespace MainGame
{
    public class GrenadeBonus : BonusController
    {
        [SerializeField] private float _radius;
        [SerializeField] private int _damage;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerBehaviour player))
            {
                PickUp(player);
            }
        }

        public override void Affect(PlayerBehaviour player)
        {
            var colliders = Physics2D.OverlapCircleAll(transform.position, _radius);

            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out EnemyController enemy))
                {
                    var hitStatus = enemy.TakeDamage((uint)_damage);
                    player.OnWeaponHit(hitStatus);
                }
            }
        }
    }
}
