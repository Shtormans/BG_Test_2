using System;
using System.Collections;
using UnityEngine;

namespace MainGame
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected Transform BulletSpawner;
        [SerializeField] protected WeaponStats Stats;

        protected int _clip;
        protected int _inAmmo;
        protected bool _isReloading;
        protected bool _canShoot;
        protected float _timeBeforeLastFire;

        public event Action<float> Reloaded;

        protected void Init()
        {
            _inAmmo = Stats.StartAmount;
            _clip = Stats.Clip;

            _isReloading = false;
            _canShoot = false;
        }

        public void CreateBullet(Vector3 position, Quaternion direction)
        {
            var bullet = Instantiate(Stats.Bullet, BulletSpawner.position, direction);

            bullet.StartMoving(Stats.BulletSpeed, Stats.TimeBeforeDisappear);
            bullet.Hit += OnBulletHit;
        }

        public void Reload()
        {
            StartCoroutine(AwaitReload());
        }

        public void Shoot()
        {
            if (_isReloading || _clip <= 0 || _timeBeforeLastFire + Stats.FireRate > Time.time)
            {
                _timeBeforeLastFire = Time.time;
                return;
            }

            Fire();
            _timeBeforeLastFire += Stats.FireRate;
            _clip -= 1;
            if (_clip == 0)
            {
                Reloaded?.Invoke(Stats.ReloadTime);
                Reload();
            }
        }

        protected abstract void Fire();
        protected abstract void OnBulletHit(Entity entity);

        private IEnumerator AwaitReload()
        {
            _isReloading = false;

            yield return new WaitForSeconds(Stats.ReloadTime);

            if (_inAmmo - Stats.Clip < 0)
            {
                _inAmmo = 0;
                _clip = _inAmmo - Stats.Clip;
            }
            else
            {
                _inAmmo -= Stats.Clip;
                _clip = Stats.Clip;
            }

            _isReloading = true;
        }
    }
}
