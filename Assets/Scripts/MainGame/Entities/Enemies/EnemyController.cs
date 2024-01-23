using Fusion;
using UnityEngine;
using Zenject;

namespace MainGame
{
    public abstract class EnemyController : Entity
    {
        [field: SerializeField] public float DistanceToShoot { get; protected set; }
        public Transform Target { get; protected set; }

        public Rigidbody2D Rigidbody { get; protected set; }
        private bool LastRunningState;

        protected PlayersContainer PlayersContainer;
        private EnemyContainer _enemyContainer;

        public class Factory : PlaceholderFactory<EnemyController, NetworkRunner, PlayerRef, EnemyController>
        {
        }

        public class EnemyControllerFactory : IFactory<EnemyController, NetworkRunner, PlayerRef, EnemyController>
        {
            private DiContainer _diContainer;

            public EnemyControllerFactory(DiContainer diContainer)
            {
                _diContainer = diContainer;
            }

            public EnemyController Create(EnemyController prefab, NetworkRunner runner, PlayerRef inputAuthority)
            {
                var enemy = runner.Spawn(prefab, inputAuthority: inputAuthority);
                _diContainer.Inject(enemy);

                return enemy;
            }
        }

        [Inject]
        public void Construct(PlayersContainer playersContainer, EnemyContainer enemyContainer)
        {
            Debug.Log("Injecting");
            PlayersContainer = playersContainer;
            _enemyContainer = enemyContainer;
        }

        protected override void OnDied()
        {
            _enemyContainer.RemoveEnemy(this);

            base.OnDied();
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority) return;

            LastRunningState = IsRunning;
            IsRunning = false;

            UpdateTarget();
            Move();

            UpdateRunningState();
        }

        private void UpdateRunningState()
        {
            if (IsRunning != LastRunningState)
            {
                if (!IsRunning)
                {
                    RPC_SendTrigger(AnimationTriggers.StoppedRunning);
                }
                else
                {
                    RPC_SendTrigger(AnimationTriggers.StartedRunning);
                }
            }
        }

        protected abstract void Move();

        protected void UpdateTarget()
        {
            var player = PlayersContainer.GetNearestPlayer(transform);

            Target = player != null ? player.transform : transform;
        }

        public void MoveTo(Vector3 direction)
        {
            IsRunning = true;
            Rigidbody.MovePosition(transform.position + Runner.DeltaTime * Stats.EntityData.Speed * direction);
        }
    }
}