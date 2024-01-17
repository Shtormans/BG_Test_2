using System;
using System.Collections.Generic;

namespace MainGame
{
    public class EnemyStateMachine
    {
        private Dictionary<Type, State> _states;
        private State _currentState;

        public State CurrentState => _currentState;

        public void Init(EnemyController enemy)
        {
            _states = new Dictionary<Type, State>()
            {
                {typeof(MoveState), new MoveState(enemy, enemy.Runner, this)},
                {typeof(ShootState), new ShootState(enemy, this)}
            };
        }

        public void SetState<TState>()
        {
            _currentState?.ExitState();

            _currentState = _states[typeof(TState)];
            _currentState.EnterState();
        }

        public void UseState()
        {
            _currentState?.Update();
        }
    }
}
