using Fusion;
using UnityEngine;

namespace MainGame
{
    public class SuperZombie : EnemyController
    {
        private EnemyStateMachine _stateMachine;

        public NetworkRunner NetworkRunner => Runner;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();

            _stateMachine = new EnemyStateMachine();
            _stateMachine.Init(this);
        }

        private void OnEnable()
        {
            Init();

            _stateMachine.SetState<MoveState>();
        }

        protected override void Move()
        {
            _stateMachine.UseState();
        }
    }
}
