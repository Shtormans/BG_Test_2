using UnityEngine;

namespace MainGame
{
    public abstract class EnemyController : Entity
    {
        [field:SerializeField] public float DistanceToShoot { get; protected set; }
        [field:SerializeField] public PlayersContainer PlayersContainer { get; protected set; }
        public Transform Target { get; protected set; }

        public Rigidbody2D Rigidbody { get; protected set; }

        public override void FixedUpdateNetwork()
        {
            UpdateTarget();
            Move();
        }

        public void SetPlayersContainer(PlayersContainer playersContainer)
        {
            PlayersContainer = playersContainer;
        }

        protected abstract void Move();
        protected void UpdateTarget()
        {
            var player = PlayersContainer.GetNearestPlayer(transform);

            Target = player.transform != null ? player.transform : transform;
            Debug.Log(Target);
        }
    }
}