using Fusion;
using UnityEngine;

namespace MainGame
{
    public abstract class Weapon : NetworkBehaviour
    {
        [SerializeField] protected WeaponStats Stats;
        protected float _lastFireTime;

        protected virtual void Init()
        {
            _lastFireTime = -Stats.FireRate;
        }

        public virtual void Shoot()
        {
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

        protected abstract void Fire();
        protected abstract void OnHit(Entity entity);
    }
}
