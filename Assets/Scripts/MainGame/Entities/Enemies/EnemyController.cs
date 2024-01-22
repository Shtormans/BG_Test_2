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

        [Inject]
        private void Construct(PlayersContainer playersContainer)
        {
            PlayersContainer = playersContainer;
        }

        public class Factory : PlaceholderFactory<EnemyController>
        {
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