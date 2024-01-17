using Fusion;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public abstract class State 
    {
        protected NetworkRunner Runner;
        protected EnemyController Enemy;
        protected EnemyStateMachine StateMachine;
        protected List<StateTransition> _transitions;

        public State(EnemyStateMachine stateMachine, EnemyController enemy, NetworkRunner runner)
        {
            Enemy = enemy;
            Runner = runner;
            StateMachine = stateMachine;
        }

        public virtual void EnterState() { }
        public virtual void ExitState() { }
        public abstract void Update();

        protected void CheckTransitions()
        {
            foreach (var transition in _transitions)
            {
                if (transition.CheckCondition(Enemy))
                {
                    transition.ChangeState();
                }
            }
        }
    }
}
