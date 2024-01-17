using System.Diagnostics;

namespace MainGame
{
    public class Bow : Weapon
    {
        private void OnEnable()
        {
            Init();
        }

        protected override void OnBulletHit(Entity entity)
        {
            entity.TakeDamage((uint)Stats.Damage);
        }

        protected override void Fire()
        {
            CreateBullet(BulletSpawner.position, transform.rotation);
        }
    }
}
