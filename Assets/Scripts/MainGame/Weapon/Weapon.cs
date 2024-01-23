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

        public class Factory : PlaceholderFactory<Weapon, Weapon>
        {

        }

        public class WeaponFactory : IFactory<Weapon, Weapon>
        {
            private DiContainer _diContainer;

            public WeaponFactory(DiContainer diContainer)
            {
                _diContainer = diContainer;
            }

            public Weapon Create(Weapon prefab)
            {
                _diContainer.Inject(prefab);

                return prefab;
            }
        }

        [Inject]
        public void Construct(NetworkObjectsContainer networkObjectsContainer)
        {
            networkObjectsContainer.AddObject(this);
        }

        public override void Spawned()
        {
            PlayerInjectionManager.InjectIntoWeapon(this);
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
