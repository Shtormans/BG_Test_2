using Fusion;
using MainMenu;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MainGame
{
    public class NetworkSpawner : NetworkRunnerCallbacks
    {
        [SerializeField] private PlayerFabric _playerFabric;
        [SerializeField] private ButtonsController _buttonsController;

        private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
        private NetworkRunner _runner;
        private PlayerRef _currentPlayer;

        public NetworkRunner Runner => _runner;
        public PlayerRef CurrentPlayer => _currentPlayer;

        private PlayersContainer _playersContainer;
        private NetworkManager _networkManager;
        private MultisceneItemsTransfer _transfer;

        [Inject]
        private void Construct(PlayersContainer playersContainer, NetworkManager networkManager, MultisceneItemsTransfer transfer)
        {
            _playersContainer = playersContainer;
            _networkManager = networkManager;
            _transfer = transfer;
        }

        private void Awake()
        {
            _runner = _networkManager.Runner;
            _runner.AddCallbacks(this);
        }

        public override void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                NetworkObject networkPlayerObject = _playerFabric.Create(runner, player).GetComponent<NetworkObject>();
                _spawnedCharacters.Add(player, networkPlayerObject);

                _currentPlayer = player;
            }
        }

        public override void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
            {
                runner.Despawn(networkObject);
                _spawnedCharacters.Remove(player);

                var playerBehaviour = networkObject.gameObject.GetComponent<PlayerBehaviour>();
                _playersContainer.RemovePlayer(playerBehaviour);
            }

            if (!runner.IsServer)
            {
                _buttonsController.ExitToMainMenu();
            }
        }
    }
}
