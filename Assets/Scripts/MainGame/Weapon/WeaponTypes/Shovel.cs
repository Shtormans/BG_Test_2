namespace MainGame
{
    public class Shovel : Weapon
    {
        private void OnEnable()
        {
            Init();
        }

        protected override void OnHit(Entity entity)
        {
            entity.TakeDamage((uint)Stats.Damage);
        }

        protected override void Fire()
        {
            
        }
    }
}
