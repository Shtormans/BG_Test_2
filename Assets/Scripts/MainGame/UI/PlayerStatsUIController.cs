using Fusion;
using TMPro;
using UnityEngine;

namespace MainGame
{
    public class PlayerStatsUIController : NetworkBehaviour
    {
        [SerializeField] private TextMeshProUGUI _healthText;
        [SerializeField] private TextMeshProUGUI _weaponText;
        [SerializeField] private TextMeshProUGUI _reloadingTimeText;

        private PlayerBehaviour _player;

        public void SetPlayer(PlayerBehaviour player)
        {
            _player = player;

            player.Hit += OnPlayerHit;
            player.Weapon.Fired += OnPlayerShoot;
            (player.Weapon as WeaponWithBullets).StartedReloading += OnStartedReloading;
            (player.Weapon as WeaponWithBullets).StoppedReloading += OnStoppedReloading;
            (player.Weapon as WeaponWithBullets).ReloadingTimeChanged += OnReloadingTimeChanged;
            _healthText.text = player.Stats.EntityData.Health.ToString();
            OnPlayerShoot();
        }

        private void OnReloadingTimeChanged(float reloadtimeLeft)
        {
            _reloadingTimeText.text = ConvertToReloadingTime(reloadtimeLeft);
        }

        private void OnStartedReloading(float reloadTime)
        {
            ChangeToReloadText();
            _reloadingTimeText.text = ConvertToReloadingTime(reloadTime);
        }

        private void OnStoppedReloading()
        {
            ChangeToBulletsLeftText();

            OnPlayerShoot();
        }

        private void OnPlayerShoot()
        {
            var weaponWithBullets = _player.Weapon as WeaponWithBullets;

            _weaponText.text = ConvertWeaponToString(weaponWithBullets);
        }

        private void ChangeToReloadText()
        {
            _weaponText.gameObject.SetActive(false);
            _reloadingTimeText.gameObject.SetActive(true);
        }

        private string ConvertToReloadingTime(float timeInSeconds)
        {
            int minutes = (int)(timeInSeconds / 60f);
            int seconds = (int)(timeInSeconds - 60f * minutes);

            return $"{minutes}:{seconds.ToString("00")}";
        }

        private void ChangeToBulletsLeftText()
        {
            _reloadingTimeText.gameObject.SetActive(false);
            _weaponText.gameObject.SetActive(true);
        }

        private void OnPlayerHit(EntityHitStatus hitStatus)
        {
            _healthText.text = hitStatus.HealthRemained.ToString();
        }

        private string ConvertWeaponToString(WeaponWithBullets weapon)
        {
            return $"{weapon.Clip}/{weapon.InAmmo}";
        }
    }
}
