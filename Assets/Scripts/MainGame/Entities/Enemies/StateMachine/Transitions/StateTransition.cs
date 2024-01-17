namespace MainGame
{
    public abstract class StateTransition
    {
        protected EnemyStateMachine StateMachine;
        protected EnemyController EnemyController;

        public StateTransition(EnemyController enemyController, EnemyStateMachine stateMachine)
        {
            EnemyController = enemyController;
            StateMachine = stateMachine;
        }

        public abstract void ChangeState();
        public abstract bool CheckCondition(EnemyController enemy);
    }
}
