using Fusion;
using System;
using UnityEngine;

namespace MainGame
{
    public class PlayerRPCHandler : NetworkBehaviour
    {
        public event Action<EntityHitStatus> PlayerWasHit;
        public event Action<PlayerGameStats> PlayerKilled;
        public event Action<Health> PlayerHealthAmountChanged;
        public event Action<WeaponBulletStatus> PlayerWeaponBulletsAmountChanged;
        public event Action<float> PlayerWeaponStartedReloading;
        public event Action<WeaponBulletStatus> PlayerWeaponStoppedReloading;
        public event Action<float> PlayerWeaponReloadingTimeChanged;

        private PlayerBehaviour _player;

        private void Awake()
        {
            var player = GetComponent<PlayerBehaviour>();
            _player = player;

            player.WasHit += RPC_PlayerWasHit;
            player.Killed += RPC_PlayerKilled;
            player.HealthAmountChanged += RPC_PlayerHealthAmountChanged;
            player.WeaponChanged += OnWeaponChanged;
        }

        public void UseCurrentPlayerHealthStatusRPC()
        {
            RPC_GetCurrentHealthStatus();
        }

        public void UseCurrentWeaponBulletsStatusRPC()
        {
            RPC_GetCurrentWeaponBulletsStatus();
        }

        private void OnWeaponChanged(WeaponWithBullets weapon)
        {
            weapon.BulletsAmountChanged += RPC_PlayerWeaponBulletsAmountChanged;
            weapon.StartedReloading += RPC_PlayerWeaponStartedReloading;
            weapon.StoppedReloading += RPC_PlayerWeaponStoppedReloading;
            weapon.ReloadingTimeChanged += RPC_PlayerWeaponReloadingTimeChanged;
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_PlayerWasHit(EntityHitStatus hitStatus)
        {
            PlayerWasHit?.Invoke(hitStatus);
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_PlayerKilled(PlayerGameStats gameStats)
        {
            PlayerKilled?.Invoke(gameStats);
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_PlayerHealthAmountChanged(Health health)
        {
            PlayerHealthAmountChanged?.Invoke(health);
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_PlayerWeaponBulletsAmountChanged(WeaponBulletStatus bulletStatus)
        {
            PlayerWeaponBulletsAmountChanged?.Invoke(bulletStatus);
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_PlayerWeaponStartedReloading(float secondsLeft)
        {
            PlayerWeaponStartedReloading?.Invoke(secondsLeft);
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_PlayerWeaponStoppedReloading(WeaponBulletStatus bulletStatus)
        {
            PlayerWeaponStoppedReloading?.Invoke(bulletStatus);
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_PlayerWeaponReloadingTimeChanged(float secondsLeft)
        {
            PlayerWeaponReloadingTimeChanged?.Invoke(secondsLeft);
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_GetCurrentHealthStatus()
        {
            RPC_PlayerHealthAmountChanged(_player.Health);
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_GetCurrentWeaponBulletsStatus()
        {
            RPC_PlayerWeaponBulletsAmountChanged(_player.PlayerWeapon.BulletStatus);
        }
    }
}
