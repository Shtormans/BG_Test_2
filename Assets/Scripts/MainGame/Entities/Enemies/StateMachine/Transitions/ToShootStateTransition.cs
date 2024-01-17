using UnityEngine;

namespace MainGame
{
    public class ToShootStateTransition : StateTransition
    {
        public ToShootStateTransition(EnemyController enemyController, EnemyStateMachine stateMachine)
            : base(enemyController, stateMachine)
        {
        }

        public override void ChangeState()
        {
            StateMachine.SetState<ShootState>();
        }

        public override bool CheckCondition(EnemyController enemy)
        {
            return Vector3.Distance(enemy.transform.position, enemy.Target.position) <= EnemyController.DistanceToShoot;
        }
    }
}
