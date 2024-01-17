using Fusion;
using UnityEngine;

namespace MainGame
{
    public class MoveState : State
    {
        private PlayersContainer _playersContainer;

        public MoveState(EnemyController enemy, NetworkRunner runner, EnemyStateMachine stateMachine)
            : base(stateMachine, enemy, runner)
        {
            _playersContainer = new PlayersContainer();
        }

        public override void Update()
        {
            var target = _playersContainer.GetNearestPlayer(Enemy.transform);

            if (Enemy.DistanceToShoot <= Vector3.Distance(target.transform.position, Enemy.transform.position))
            {
                StateMachine.SetState<ShootState>();
            }

            var direction = (target.transform.position - Enemy.transform.position).normalized;
            Enemy.Rigidbody.MovePosition(Runner.DeltaTime * Enemy.Stats.EnemyData.Speed * direction);
        }
    }
}
