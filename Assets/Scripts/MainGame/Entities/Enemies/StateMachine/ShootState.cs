using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class ShootState : State
    {
        public ShootState(EnemyController enemy, EnemyStateMachine stateMachine)
            : base(stateMachine, enemy, enemy.Runner)
        {
            _transitions = new List<StateTransition>()
            {
                new ToMoveStateTransition(enemy, stateMachine)
            };
        }

        public override void Update()
        {
            CheckTransitions();

            var direction = (Enemy.Target.transform.position - Enemy.transform.position).normalized;
            Rotate(direction);

            Enemy.Weapon.Shoot();
        }

        private void Rotate(Vector3 direction)
        {
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Enemy.Weapon.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
