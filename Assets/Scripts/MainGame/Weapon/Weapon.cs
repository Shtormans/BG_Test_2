using Fusion;
using System;
using UnityEngine;
using Zenject;

namespace MainGame
{
    public abstract class Weapon : NetworkBehaviour
    {
        [SerializeField] protected WeaponStats Stats;
        protected float _lastFireTime;

        public event Action<EntityHitStatus> Hit;
        public event Action Fired;

        [Inject]
        private NetworkObjectsContainer _networkObjectsContainer;

        public override void Spawned()
        {
            _networkObjectsContainer.AddObject(this);
        }

        protected virtual void Init()
        {
            _lastFireTime = -Stats.FireRate;
        }

        public void Rotate(Quaternion rotation)
        {
            RPC_RotateWeapon(rotation);
        }

        public virtual void Shoot()
        {
            if (!TimePassedBeforeNextShoot())
            {
                return;
            }

            if (HasStateAuthority)
            {
                Fire();
            }

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

        protected void TriggerHitEvent(EntityHitStatus hitStatus)
        {
            Hit?.Invoke(hitStatus);
        }

        protected abstract void Fire();
        protected abstract void OnHit(Entity entity);

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_RotateWeapon(Quaternion rotation, RpcInfo info = default)
        {
            transform.rotation = rotation;
        }
    }
}
