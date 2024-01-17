namespace MainGame
{
    public abstract class Transition
    {
        public abstract State State { get; }

        public abstract bool CheckCondition();
    }
}
