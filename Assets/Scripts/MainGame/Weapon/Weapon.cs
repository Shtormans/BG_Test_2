using Fusion;
using UnityEngine;

namespace MainGame
{
    public abstract class Weapon : NetworkBehaviour
    {
        [SerializeField] protected WeaponStats Stats;
        protected float _lastFireTime;

        public override void Spawned()
        {
            NetworkObjectsContainer.Instance.AddObject(this);
        }

        protected virtual void Init()
        {
            _lastFireTime = -Stats.FireRate;
        }

        public virtual void Shoot()
        {
            if (!HasStateAuthority)
            {
                return;
            }

            if (!TimePassedBeforeNextShoot())
            {
                return;
            }

            Fire();

            _lastFireTime = Time.time;
        }

        protected bool TimePassedBeforeNextShoot()
        {
            return Time.time - _lastFireTime > Stats.FireRate;
        }

        protected bool IsFriendlyFire(Entity entity)
        {
            return entity.Weapon != this;
        }

        protected abstract void Fire();
        protected abstract void OnHit(Entity entity);
    }
}
