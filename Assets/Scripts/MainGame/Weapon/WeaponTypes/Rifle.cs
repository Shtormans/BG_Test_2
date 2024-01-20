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
            var hitStatus = entity.TakeDamage((uint)Stats.Damage);
            TriggerHitEvent(hitStatus);
        }

        protected override void Fire()
        {
            CreateBullet(BulletSpawner.position, transform.rotation);
        }
    }
}
