using Fusion;
using System;
using UnityEngine;

namespace MainGame
{
    public abstract class Weapon : NetworkBehaviour
    {
        [SerializeField] protected WeaponStats Stats;
        protected float _lastFireTime;

        public event Action Fired;

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
            TriggerFireEvent();

            _lastFireTime = Time.time;
        }

        protected void TriggerFireEvent()
        {
            Fired?.Invoke();
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
