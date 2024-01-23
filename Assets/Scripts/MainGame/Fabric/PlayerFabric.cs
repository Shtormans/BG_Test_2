using Cinemachine;
using Fusion;
using System;
using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;

namespace MainGame
{
    public class PlayerFabric : MonoBehaviour
    {
        [SerializeField] private PlayerBehaviour prefab;
        [SerializeField] private WeaponsContainer _weaponsContainer;
        [SerializeField] private SkinsContainer _skinsContainer;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private PlayerStatsUIController _playerStatsUIController;
        [SerializeField] private WorldMonitor _worldMonitor;

        [Inject] private NetworkObjectsContainer _networkObjectsContainer;
        [Inject] private PlayersContainer _playersContainer;
        private PlayerBehaviour _currentPlayer;

        public PlayerBehaviour Create(NetworkRunner runner, PlayerRef inputAuthority)
        {
            Debug.Log("Before spawning");
            try
            {
                var player = runner.Spawn(prefab, inputAuthority: inputAuthority);
                Debug.Log("Here 1");
                player.Died += _worldMonitor.OnPlayerDied;
                Debug.Log("Here 2");
                _currentPlayer = player;

                Debug.Log(_currentPlayer);
                Debug.Log("After spawning");

                return player;
            }catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }

            return null;
        }

        public PlayerBehaviour CreatePlayerBody(SpawnPlayerStructure playerStructure)
        {
            Debug.Log(_currentPlayer);
            var weapon = _currentPlayer.Runner.Spawn(_weaponsContainer.TakeUniqueWeapon());
            var skin = _currentPlayer.Runner.Spawn(_skinsContainer.TakeSkinById(playerStructure.SkinId));

            var playerBody = new PlayerBody()
            {
                WeaponId = weapon.Id,
                SpriteId = skin.Id
            };
            
            _currentPlayer = PlayerBuilder.CreateBuilder(_currentPlayer)
                .AddPlayerBody(playerBody)
                .Build();

            return _currentPlayer;
        }

        public void UpdateSharedPlayer(PlayerBehaviour player)
        {
            _networkObjectsContainer.TryGetObjectById(player.PlayerBody.WeaponId, out Weapon weapon);
            _networkObjectsContainer.TryGetObjectById(player.PlayerBody.SpriteId, out AnimatedSkin skin); ;

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
