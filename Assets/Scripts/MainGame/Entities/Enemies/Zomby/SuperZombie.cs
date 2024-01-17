using Fusion;
using UnityEngine;

namespace MainGame
{
    public class SuperZombie : EnemyController
    {
        private Rigidbody2D _rigidbody;
        private EnemyStateMachine _stateMachine;

        public Rigidbody2D Rigidbody => _rigidbody;
        public NetworkRunner NetworkRunner => Runner;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

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
