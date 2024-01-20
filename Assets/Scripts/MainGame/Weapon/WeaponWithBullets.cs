using System;
using System.Collections;
using UnityEngine;

namespace MainGame
{
    public abstract class WeaponWithBullets : Weapon
    {
        [SerializeField] protected Transform BulletSpawner;
        private float _timeBetweenReloading = 1f;

        protected int _clip;
        protected int _inAmmo;
        protected bool _isReloading;

        public int Clip => _clip;
        public int InAmmo => _inAmmo;

        public event Action<float> StartedReloading;
        public event Action<float> ReloadingTimeChanged;
        public event Action StoppedReloading;
        public event Action BulletsAmountChanged;

        protected override void Init()
        {
            _inAmmo = Stats.StartAmount;
            _clip = Stats.Clip;
            _lastFireTime = -Stats.FireRate;

            _isReloading = false;
        }

        public void AddClips(int clips)
        {
            _inAmmo += clips * Stats.Clip;
            BulletsAmountChanged?.Invoke();
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

            if (HasStateAuthority)
            {
                Fire();
            }

            if (!Stats.InfiniteAmmo)
            {
                _clip -= 1;
            }

            TriggerFireEvent();

            _lastFireTime = Time.time;
            if (!Stats.InfiniteAmmo)
            {
                HandleBullets();
            }
        }

        private void HandleBullets()
        {
            if (_clip == 0 && _inAmmo > 0)
            {
                StartedReloading?.Invoke(Stats.ReloadTime);
                Reload();
            }
        }

        private bool CanShoot()
        {
            return !_isReloading && _clip > 0 && TimePassedBeforeNextShoot();
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

            _isReloading = false;
            StoppedReloading?.Invoke();
        }
    }
}
