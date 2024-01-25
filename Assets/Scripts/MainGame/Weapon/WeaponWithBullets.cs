using Fusion;
using System;
using System.Collections;
using UnityEngine;

namespace MainGame
{
    public abstract class WeaponWithBullets : Weapon
    {
        [SerializeField] protected Transform BulletSpawner;
        private float _timeBetweenReloading = 1f;

        private WeaponBulletStatus _bulletStatus;
        protected bool _isReloading;

        public int Clip => _bulletStatus.InClip;
        public int InAmmo => _bulletStatus.InAmmo;
        public WeaponBulletStatus BulletStatus => _bulletStatus;

        public event Action<float> StartedReloading;
        public event Action<float> ReloadingTimeChanged;
        public event Action<WeaponBulletStatus> StoppedReloading;
        public event Action<WeaponBulletStatus> BulletsAmountChanged;

        protected override void Init()
        {
            _bulletStatus = new WeaponBulletStatus();

            _bulletStatus.InAmmo = Stats.StartAmount;
            _bulletStatus.InClip = Stats.Clip;
            _lastFireTime = -Stats.FireRate;

            _isReloading = false;
        }

        public void AddClips(int clips)
        {
            _bulletStatus.InAmmo += clips * Stats.Clip;

            if (HasStateAuthority)
            {
                BulletsAmountChanged?.Invoke(_bulletStatus);

                if (!_isReloading)
                {
                    HandleBullets();
                }
            }
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

            if (!HasStateAuthority)
            {
                return;
            }

            Fire();

            if (!Stats.InfiniteAmmo)
            {
                _bulletStatus.InClip -= 1;
            }

            BulletsAmountChanged?.Invoke(_bulletStatus);
            TriggerFireEvent();

            _lastFireTime = Time.time;
            if (!Stats.InfiniteAmmo)
            {
                HandleBullets();
            }
        }

        private void HandleBullets()
        {
            if (_bulletStatus.InClip == 0 && _bulletStatus.InAmmo > 0)
            {
                StartedReloading?.Invoke(Stats.ReloadTime);
                Reload();
            }
        }

        private bool CanShoot()
        {
            return !_isReloading && _bulletStatus.InClip > 0 && TimePassedBeforeNextShoot();
        }

        private IEnumerator AwaitReload()
        {
            _isReloading = true;
            float timePassed = 0f;

            while (timePassed <= Stats.ReloadTime)
            {
                yield return new WaitForSeconds(_timeBetweenReloading);

                timePassed += _timeBetweenReloading;
                ReloadingTimeChanged?.Invoke(Stats.ReloadTime - timePassed);
            }

            if (_bulletStatus.InAmmo - Stats.Clip < 0)
            {
                _bulletStatus.InClip = _bulletStatus.InAmmo;
                _bulletStatus.InAmmo = 0;
            }
            else
            {
                _bulletStatus.InAmmo -= Stats.Clip;
                _bulletStatus.InClip = Stats.Clip;
            }

            _isReloading = false;
            StoppedReloading?.Invoke(_bulletStatus);
        }
    }
}
