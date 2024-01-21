using Fusion;
using System;
using UnityEngine;

namespace MainGame
{
    public abstract class Weapon : NetworkBehaviour
    {
        [SerializeField] protected WeaponStats Stats;
        protected float _lastFireTime;

        public event Action<EntityHitStatus> Hit;
        public event Action Fired;

        public override void Spawned()
        {
            NetworkObjectsContainer.Instance.AddObject(this);
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
            RPC_Fire();
        }

        protected bool TimePassedBeforeNextShoot()
        {
            return Time.time - _lastFireTime > Stats.FireRate;
        }

        protected void TriggerHitEvent(EntityHitStatus hitStatus)
        {
            RPC_Hit(hitStatus);
        }

        protected abstract void Fire();
        protected abstract void OnHit(Entity entity);

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_RotateWeapon(Quaternion rotation, RpcInfo info = default)
        {
            transform.rotation = rotation;
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_Fire(RpcInfo info = default)
        {
            Fired?.Invoke();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_Hit(EntityHitStatus hitStatus, RpcInfo info = default)
        {
            Hit?.Invoke(hitStatus);
        }
    }
}
