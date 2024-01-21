using Cinemachine;
using Fusion;
using UnityEngine;

namespace MainGame
{
    public class PlayerFabric : MonoBehaviour
    {
        [SerializeField] private PlayerBehaviour prefab;
        [SerializeField] private WeaponsContainer _weaponsContainer;
        [SerializeField] private SkinsContainer _skinsContainer;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private PlayerStatsUIController _playerStatsUIController;

        public PlayerBehaviour Create(NetworkRunner runner, PlayerRef inputAuthority)
        {
            var player = runner.Spawn(prefab, inputAuthority: inputAuthority);
            var weapon = runner.Spawn(_weaponsContainer.TakeUniqueWeapon(), inputAuthority: inputAuthority);
            var skin = runner.Spawn(_skinsContainer.TakeUniqueSkin(), inputAuthority: inputAuthority);

            var playerBody = new PlayerBody()
            {
                WeaponId = weapon.Id,
                SpriteId = skin.Id
            };

            player = PlayerBuilder.CreateBuilder(player)
                .AddPlayerBody(playerBody)
                .AddSkin(skin)
                .AddWeapon(weapon)
                .Build();

            return player;
        }

        public void UpdateSharedPlayer(PlayerBehaviour player)
        {
            NetworkObjectsContainer.Instance.TryGetObjectById(player.PlayerBody.WeaponId, out Weapon weapon);
            NetworkObjectsContainer.Instance.TryGetObjectById(player.PlayerBody.SpriteId, out AnimatedSkin skin);

            PlayerBuilder.CreateBuilder(player)
                .AddWeapon(weapon)
                .AddSkin(skin)
                .Build();
        }

        public void UpdateInputPlayer(PlayerBehaviour player)
        {
            PlayerBuilder.CreateBuilder(player)
                .AddCamera(_virtualCamera)
                .AddUIController(_playerStatsUIController)
                .Build();

            NetworkObjectsContainer.Instance.TryGetObjectById(player.PlayerBody.SpriteId, out AnimatedSkin skin);
        }

        public bool IsAllPlayerPartsSpawned(PlayerBehaviour player)
        {
            return NetworkObjectsContainer.Instance.TryGetObjectById(player.PlayerBody.WeaponId, out Weapon _)
                && NetworkObjectsContainer.Instance.TryGetObjectById(player.PlayerBody.SpriteId, out AnimatedSkin _);
        }
    }
}
