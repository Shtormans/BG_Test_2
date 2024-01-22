using Cinemachine;
using Fusion;
using UnityEngine;
using Zenject;

namespace MainGame
{
    public class PlayerFabric : MonoBehaviour
    {
        [SerializeField] private PlayerBehaviour prefab;
        [SerializeField] private WeaponsContainer _weaponsContainer;
        [SerializeField] private SkinsContainer _skinsContainer;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private PlayerStatsUIController _playerStatsUIController;

        [Inject]
        private NetworkObjectsContainer _networkObjectsContainer;

        public PlayerBehaviour Create(NetworkRunner runner, PlayerRef inputAuthority)
        {
            var player = runner.Spawn(prefab, inputAuthority: inputAuthority);
            var weapon = runner.Spawn(_weaponsContainer.TakeUniqueWeapon(), inputAuthority: inputAuthority);
            var skin = runner.Spawn(_skinsContainer.TakeSkin(), inputAuthority: inputAuthority);

            var playerBody = new PlayerBody()
            {
                WeaponId = weapon.Id,
                SpriteId = skin.Id
            };

            player = PlayerBuilder.CreateBuilder(player)
                .AddPlayerBody(playerBody)
                .Build();

            return player;
        }

        public void UpdateSharedPlayer(PlayerBehaviour player)
        {
            _networkObjectsContainer.TryGetObjectById(player.PlayerBody.WeaponId, out Weapon weapon);
            _networkObjectsContainer.TryGetObjectById(player.PlayerBody.SpriteId, out AnimatedSkin skin);

            PlayerBuilder.CreateBuilder(player)
                .AddSkin(skin)
                .AddWeapon(weapon)
                .Build();
        }

        public void UpdateInputPlayer(PlayerBehaviour player)
        {
            PlayerBuilder.CreateBuilder(player)
                .AddCamera(_virtualCamera)
                .AddUIController(_playerStatsUIController)
                .Build();

            _networkObjectsContainer.TryGetObjectById(player.PlayerBody.SpriteId, out AnimatedSkin skin);
        }

        public bool IsAllPlayerPartsSpawned(PlayerBehaviour player)
        {
            return _networkObjectsContainer.TryGetObjectById(player.PlayerBody.WeaponId, out Weapon _)
                && _networkObjectsContainer.TryGetObjectById(player.PlayerBody.SpriteId, out AnimatedSkin _);
        }
    }
}
