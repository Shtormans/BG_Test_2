using Fusion;
using System;
using UnityEngine;

namespace MainGame
{
    public abstract class Entity : NetworkBehaviour
    {
        [SerializeField] public EntityStats Stats; 
        private Health _health;

        public event Action Died;

        protected void Init()
        {
            _health = new Health(Stats.EnemyData.Health);
        }

        public void TakeDamage(uint damage)
        {
            _health.TakeDamage((int)damage);

            if (_health.IsAlive())
            {
                Died?.Invoke();
            }
        }
    }
}
