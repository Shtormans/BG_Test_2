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
        public Health health => _health;

        public event Action<EntityHitStatus> WasHit;
        public event Action<AnimationTriggers> AnimationTriggered;
        public event Action<int> HealthAmountChanged;

        private void OnCollisionExit2D(Collision2D collision)
        {
            _rigidbody.velocity = Vector3.zero;
        }

        protected virtual void Died()
        {
            Runner.Spawn(DeadCopy,
                        transform.position,
                        Quaternion.identity,
                        Object.InputAuthority);

            Runner.Despawn(Object);
        }

        protected void Init()
        {
            _health = new Health(Stats.EntityData.Health);
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        protected void AddHealth(int health)
        {
            _health.AddHealth((uint)health);
            HealthAmountChanged?.Invoke(_health.CurrentHealth);
        }

        public EntityHitStatus TakeDamage(uint damage)
        {
            if (!HasInputAuthority)
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
                Died();
            }
            else
            {
                RPC_SendTrigger(AnimationTriggers.Hit);
            }

            HealthAmountChanged?.Invoke(_health.CurrentHealth);
            WasHit?.Invoke(hitStatus);

            return hitStatus;
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
        protected void RPC_SendTrigger(AnimationTriggers trigger, RpcInfo info = default)
        {
            RPC_RelayTrigger(trigger, info.Source);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
        protected void RPC_RelayTrigger(AnimationTriggers trigger, PlayerRef messageSource)
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
