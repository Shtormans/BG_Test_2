using UnityEngine;

namespace MainGame
{
    public class ToMoveStateTransition : StateTransition
    {
        public ToMoveStateTransition(EnemyController enemyController, EnemyStateMachine stateMachine)
            : base(enemyController, stateMachine)
        {
        }

        public override void ChangeState()
        {
            StateMachine.SetState<MoveState>();
        }

        public override bool CheckCondition(EnemyController enemy)
        {
            return Vector3.Distance(enemy.transform.position, enemy.Target.position) > EnemyController.DistanceToShoot;
        }
    }
}
