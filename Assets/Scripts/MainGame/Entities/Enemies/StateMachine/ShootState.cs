using Fusion;
using UnityEngine;

namespace MainGame
{
    public class ShootState : State
    {
        private Weapon _weapon;
        private PlayersContainer _playersContainer;

        public ShootState(EnemyController enemy, EnemyStateMachine stateMachine)
            : base(stateMachine, enemy, enemy.Runner)
        {
            _playersContainer = new PlayersContainer();
        }

        public override void Update()
        {
            var target = _playersContainer.GetNearestPlayer(Enemy.transform);

            if (Enemy.DistanceToShoot > Vector3.Distance(target.transform.position, Enemy.transform.position))
            {
                StateMachine.SetState<MoveState>();
            }

            _weapon.Shoot();
        }
    }
}
