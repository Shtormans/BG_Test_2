using System;
using System.Collections;
using UnityEngine;

namespace MainGame
{
    public abstract class WeaponWithBullets : Weapon 
    {
        [SerializeField] protected Transform BulletSpawner;

        protected int _clip;
        protected int _inAmmo;
        protected bool _isReloading;

        public event Action<float> Reloaded;

        protected override void Init()
        {
            _inAmmo = Stats.StartAmount;
            _clip = Stats.Clip;
            _lastFireTime = -Stats.FireRate;

            _isReloading = false;
        }

        public void CreateBullet(Vector3 position, Quaternion direction)
        {
            var bullet = Runner.Spawn(Stats.Bullet, position, direction, Object.InputAuthority);

            bullet.StartMoving(Stats.BulletSpeed, Stats.TimeBeforeDisappear);
            bullet.Hit += OnHit;
        }

        public void Reload()
        {
            StartCoroutine(AwaitReload());
        }

        public override void Shoot()
        {
            if (!CanShoot())
            {
                return;
            }

            Fire();

            _lastFireTime = Time.time;
            if (!Stats.InfiniteAmmo)
            {
                HandleBullets();
            }
        }

        private void HandleBullets()
        {
            _clip -= 1;
            if (_clip == 0)
            {
                Reloaded?.Invoke(Stats.ReloadTime);
                Reload();
            }
        }

        private bool CanShoot()
        {
            return !_isReloading && _clip > 0 && TimePassedBeforeNextShoot();
        }

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
