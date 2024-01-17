using Fusion;
using System;
using UnityEngine;

namespace MainGame
{
    public abstract class Entity : NetworkBehaviour
    {
        [field: SerializeField] public Weapon Weapon { get; protected set; }
        [SerializeField] public EntityStats Stats;
        private Health _health;

        public event Action Died;
        public event Action Hit;

        protected void Init()
        {
            _health = new Health(Stats.EntityData.Health);
        }

        public void TakeDamage(uint damage)
        {
            _health.TakeDamage((int)damage);
            Hit?.Invoke();

            if (_health.IsAlive())
            {
                Died?.Invoke();
            }
        }
    }
}
