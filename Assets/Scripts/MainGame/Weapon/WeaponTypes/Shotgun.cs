using UnityEngine;

namespace MainGame
{
    public class Shotgun : Weapon
    {
        [SerializeField] private int _bulletsAmount = 3;
        [SerializeField] private float _bulletAngle = 9f;

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
            var degrees = _bulletAngle * (_bulletsAmount / 2f);
            if (_bulletsAmount % 2 == 0)
            {
                degrees += _bulletAngle / 2f;
            }

            for (int i = 0; i < _bulletsAmount; i++)
            {

                var rotation = Quaternion.AngleAxis(transform.rotation.eulerAngles.z + degrees, Vector3.forward);
                CreateBullet(BulletSpawner.position, rotation);

                degrees += _bulletAngle;
            }
        }
    }
}
