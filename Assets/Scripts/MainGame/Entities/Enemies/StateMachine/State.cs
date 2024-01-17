using Fusion;
using UnityEngine;

namespace MainGame
{
    public abstract class State 
    {
        protected NetworkRunner Runner;
        protected EnemyController Enemy;
        protected EnemyStateMachine StateMachine;

        public State(EnemyStateMachine stateMachine, EnemyController Enemy, NetworkRunner runner)
        {
            Runner = runner;
            StateMachine = stateMachine;
        }

        public virtual void EnterState() { }
        public virtual void ExitState() { }
        public abstract void Update();
    }
}
