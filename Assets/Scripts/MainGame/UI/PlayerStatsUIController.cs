using TMPro;
using UnityEngine;

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

        private PlayerBehaviour _player;

        public void SetPlayer(PlayerBehaviour player)
        {
            _player = player;

            player.WasHit += OnPlayerWasHit;
            player.Killed += OnKillAmountChanged;
            player.PlayerWeapon.BulletsAmountChanged += OnPlayerShoot;
            player.PlayerWeapon.Fired += OnPlayerShoot;
            player.PlayerWeapon.StartedReloading += OnStartedReloading;
            player.PlayerWeapon.StoppedReloading += OnStoppedReloading;
            player.PlayerWeapon.ReloadingTimeChanged += OnReloadingTimeChanged;
            _waveController.NewWaveStarted += OnWaveChanged;
            _waveController.TimeToNewWaveChanged += OnWaveTimeChanged;
            _waveController.EndOfWave += OnWaveEnded;

            _healthText.text = player.Stats.EntityData.Health.ToString();
            OnPlayerShoot();
        }

        private void OnDisable()
        {
            _player.WasHit -= OnPlayerWasHit;
            _player.Killed -= OnKillAmountChanged;
            _player.PlayerWeapon.BulletsAmountChanged -= OnPlayerShoot;
            _player.PlayerWeapon.Fired -= OnPlayerShoot;
            _player.PlayerWeapon.StartedReloading -= OnStartedReloading;
            _player.PlayerWeapon.StoppedReloading -= OnStoppedReloading;
            _player.PlayerWeapon.ReloadingTimeChanged -= OnReloadingTimeChanged;
            _waveController.NewWaveStarted -= OnWaveChanged;
            _waveController.TimeToNewWaveChanged -= OnWaveTimeChanged;
            _waveController.EndOfWave -= OnWaveEnded;
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

        private void OnKillAmountChanged()
        {
            _amountOfKillsText.text = _player.GameStats.KillsAmount.ToString();
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

            OnPlayerShoot();
        }

        private void OnPlayerShoot()
        {
            _weaponText.text = ConvertWeaponToString(_player.PlayerWeapon);
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

        private void OnPlayerWasHit(EntityHitStatus hitStatus)
        {
            _healthText.text = hitStatus.HealthRemained.ToString();
        }

        private string ConvertWeaponToString(WeaponWithBullets weapon)
        {
            return $"{weapon.Clip}/{weapon.InAmmo}";
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
