using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using Zenject.SpaceFighter;

namespace MainGame
{
    public class PlayerStatsUIController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _healthText;
        [SerializeField] private TextMeshProUGUI _weaponText;
        [SerializeField] private TextMeshProUGUI _reloadingTimeText;
        [SerializeField] private TextMeshProUGUI _amountOfKillsText;
        [SerializeField] private TextMeshProUGUI _waveNumberText;
        [SerializeField] private TextMeshProUGUI _timeToNextWaveText;

        [SerializeField] private WaveController _waveController;

        private PlayerRPCHandler _rpcHandler;

        public void SetPlayer(PlayerBehaviour player)
        {
            _rpcHandler = player.RpcHandler;

            _rpcHandler.PlayerWasHit += OnPlayerWasHit;
            _rpcHandler.PlayerKilled += OnKillAmountChanged;
            _rpcHandler.PlayerHealthAmountChanged += OnHealthAmountChanged;
            _rpcHandler.PlayerWeaponBulletsAmountChanged += OnPlayerShoot;
            _rpcHandler.PlayerWeaponStartedReloading += OnStartedReloading;
            _rpcHandler.PlayerWeaponStoppedReloading += OnStoppedReloading;
            _rpcHandler.PlayerWeaponReloadingTimeChanged += OnReloadingTimeChanged;
            _waveController.NewWaveStarted += OnWaveChanged;
            _waveController.TimeToNewWaveChanged += OnWaveTimeChanged;
            _waveController.EndOfWave += OnWaveEnded;

            _rpcHandler.UseCurrentPlayerHealthStatusRPC();
            _rpcHandler.UseCurrentWeaponBulletsStatusRPC();
        }

        private void OnDisable()
        {
            _rpcHandler.PlayerWasHit -= OnPlayerWasHit;
            _rpcHandler.PlayerKilled -= OnKillAmountChanged;
            _rpcHandler.PlayerHealthAmountChanged -= OnHealthAmountChanged;
            _rpcHandler.PlayerWeaponBulletsAmountChanged -= OnPlayerShoot;
            _rpcHandler.PlayerWeaponStartedReloading -= OnStartedReloading;
            _rpcHandler.PlayerWeaponStoppedReloading -= OnStoppedReloading;
            _rpcHandler.PlayerWeaponReloadingTimeChanged -= OnReloadingTimeChanged;
            _waveController.NewWaveStarted -= OnWaveChanged;
            _waveController.TimeToNewWaveChanged -= OnWaveTimeChanged;
            _waveController.EndOfWave -= OnWaveEnded;
        }

        private void OnHealthAmountChanged(Health health)
        {
            _healthText.text = health.CurrentHealth.ToString();
        }

        private void OnWaveEnded(int waveNumber)
        {
            _waveNumberText.text = ConvertEndOfWaveNumberToString(waveNumber);
        }

        private void OnWaveChanged(int waveNumber)
        {
            _waveNumberText.text = ConvertWaveNumberToString(waveNumber);
        }

        private void OnWaveTimeChanged(float timeInSeconds)
        {
            _timeToNextWaveText.text = ConvertTimeToString(timeInSeconds);
        }

        private void OnKillAmountChanged(PlayerGameStats gameStats)
        {
            _amountOfKillsText.text = gameStats.KillsAmount.ToString();
        }

        private void OnReloadingTimeChanged(float reloadtimeLeft)
        {
            _reloadingTimeText.text = ConvertTimeToString(reloadtimeLeft);
        }

        private void OnStartedReloading(float reloadTime)
        {
            ChangeToReloadText();
            _reloadingTimeText.text = ConvertTimeToString(reloadTime);
        }

        private void OnStoppedReloading()
        {
            ChangeToBulletsLeftText();

            _rpcHandler.UseCurrentWeaponBulletsStatusRPC();
        }

        private void OnPlayerShoot(WeaponBulletStatus bulletStatus)
        {
            _weaponText.text = ConvertWeaponToString(bulletStatus);
        }

        private void OnPlayerWasHit(EntityHitStatus hitStatus)
        {
            _healthText.text = hitStatus.HealthRemained.ToString();
        }

        private void ChangeToReloadText()
        {
            _weaponText.gameObject.SetActive(false);
            _reloadingTimeText.gameObject.SetActive(true);
        }

        private string ConvertTimeToString(float timeInSeconds)
        {
            int minutes = (int)(timeInSeconds / 60f);
            int seconds = (int)(timeInSeconds - 60f * minutes);

            return $"{minutes}:{seconds:00}";
        }

        private void ChangeToBulletsLeftText()
        {
            _reloadingTimeText.gameObject.SetActive(false);
            _weaponText.gameObject.SetActive(true);
        }

        private string ConvertWeaponToString(WeaponBulletStatus bulletStatus)
        {
            return $"{bulletStatus.InClip}/{bulletStatus.InAmmo}";
        }

        private string ConvertWaveNumberToString(int waveNumber)
        {
            return $"Wave {waveNumber}";
        }

        private string ConvertEndOfWaveNumberToString(int waveNumber)
        {
            return $"Wave {waveNumber}-{waveNumber+1}";
        }
    }
}
