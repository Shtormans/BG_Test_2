namespace MainGame
{
    public class Rifle : WeaponWithBullets
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
            CreateBullet(BulletSpawner.position, transform.rotation);
        }
    }
}
