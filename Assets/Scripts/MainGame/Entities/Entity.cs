using Fusion;
using System;
using TMPro;
using UnityEngine;

namespace MainGame
{
    public abstract class Entity : NetworkBehaviour
    {
        [field: SerializeField] public Weapon Weapon { get; protected set; }
        [SerializeField] public EntityStats Stats;
        public bool IsRunning { get; protected set; }
        private Health _health;

        public event Action<EntityHitStatus> Hit;
        public event Action<AnimationTriggers> AnimationTriggered;

        protected void Init()
        {
            _health = new Health(Stats.EntityData.Health);
        }

        public EntityHitStatus TakeDamage(uint damage)
        {
            _health.TakeDamage((int)damage);

            var hitStatus = new EntityHitStatus()
            {
                Damage = (int)damage,
                HealthRemained = _health.CurrentHealth,
                Died = false
            };

            if (HasInputAuthority)
            {
                RPC_SendTrigger(AnimationTriggers.Hit);
            }

            if (!_health.IsAlive())
            {
                if (HasInputAuthority)
                {
                    RPC_SendTrigger(AnimationTriggers.Died);
                }

                hitStatus.Died = true;
            }

            Hit?.Invoke(hitStatus);

            return hitStatus;
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
        protected void RPC_SendTrigger(AnimationTriggers trigger, RpcInfo info = default)
        {
            RPC_RelayMessage(trigger, info.Source);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
        protected void RPC_RelayMessage(AnimationTriggers trigger, PlayerRef messageSource)
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
