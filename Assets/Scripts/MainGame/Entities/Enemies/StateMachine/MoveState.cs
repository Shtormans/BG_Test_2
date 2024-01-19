using Fusion;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class MoveState : State
    {
        public MoveState(EnemyController enemy, NetworkRunner runner, EnemyStateMachine stateMachine)
            : base(stateMachine, enemy, runner)
        {
            _transitions = new List<StateTransition>()
            {
                new ToShootStateTransition(enemy, stateMachine)
            };
        }

        public override void Update()
        {
            CheckTransitions();

            var direction = (Enemy.Target.transform.position - Enemy.transform.position).normalized;
            Rotate(direction);
            Move(direction);
        }

        private void Move(Vector3 direction)
        {
            Enemy.MoveTo(direction);
        }

        private void Rotate(Vector3 direction)
        {
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Enemy.Weapon.transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }
}
