using Fusion;
using System;
using TMPro;
using UnityEngine;

namespace MainGame
{
    public abstract class Entity : NetworkBehaviour
    {
        [SerializeField] protected DeadCopy DeadCopy;
        private Health _health;
        private Rigidbody2D _rigidbody;

        [field: SerializeField] public Weapon Weapon { get; protected set; }
        [field: SerializeField] public EntityStats Stats { get; protected set; }
        public bool IsRunning { get; protected set; }
        public Health Health => _health;

        public event Action EntitySpawned;
        public event Action<EntityHitStatus> WasHit;
        public event Action<AnimationTriggers> AnimationTriggered;
        public event Action<Health> HealthAmountChanged;
        public event Action Died;

        private void OnCollisionExit2D(Collision2D collision)
        {
            _rigidbody.velocity = Vector3.zero;
        }

        protected void TriggerSpawnEvent()
        {
            EntitySpawned?.Invoke();
        }

        protected virtual void OnDied()
        {
            if (!HasStateAuthority)
            {
                return;
            }

            Runner.Spawn(DeadCopy,
                        transform.position,
                        Quaternion.identity,
                        Object.InputAuthority);

            Runner.Despawn(Object);

            Died?.Invoke();
        }

        protected void Init()
        {
            _health = new Health(Stats.EntityData.Health);
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        protected void AddHealth(int health)
        {
            if (!HasStateAuthority)
            {
                return;
            }

            _health.AddHealth((uint)health);
            HealthAmountChanged?.Invoke(_health);
        }

        public EntityHitStatus TakeDamage(uint damage)
        {
            if (!HasStateAuthority)
            {
                return default;
            }

            _health.TakeDamage(damage);

            var hitStatus = new EntityHitStatus()
            {
                Damage = (int)damage,
                HealthRemained = _health.CurrentHealth,
                Died = false
            };

            if (!_health.IsAlive())
            {
                RPC_SendTrigger(AnimationTriggers.Died);

                hitStatus.Died = true;
                HealthAmountChanged?.Invoke(Health);
                WasHit?.Invoke(hitStatus);

                OnDied();
            }
            else
            {
                RPC_SendTrigger(AnimationTriggers.Hit);
                HealthAmountChanged?.Invoke(Health);
                WasHit?.Invoke(hitStatus);
            }

            return hitStatus;
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        protected void RPC_SendTrigger(AnimationTriggers trigger, RpcInfo info = default)
        {
            if (trigger == AnimationTriggers.StartedRunning)
            {
                IsRunning = true;
            }
            else if (trigger == AnimationTriggers.StoppedRunning)
            {
                IsRunning = false;
            }

            AnimationTriggered?.Invoke(trigger);
        }
    }
}
