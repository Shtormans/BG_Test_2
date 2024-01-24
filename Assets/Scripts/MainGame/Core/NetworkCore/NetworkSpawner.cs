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
        private int _amountOfPlayersInRoom = 0;
        private const int _maxPlayers = 2;

        public NetworkRunner Runner => _runner;
        public PlayerRef CurrentPlayer => _currentPlayer;
        public bool LobbyIsFull => _amountOfPlayersInRoom == _maxPlayers;

        private PlayersContainer _playersContainer;
        private NetworkManager _networkManager;

        [Inject]
        private void Construct(PlayersContainer playersContainer, NetworkManager networkManager)
        {
            _playersContainer = playersContainer;
            _networkManager = networkManager;
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
                _amountOfPlayersInRoom++;
            }
        }

        public override void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
            {
                runner.Despawn(networkObject);
                _spawnedCharacters.Remove(player);

                var playerBehaviour = networkObject.gameObject.GetComponent<PlayerBehaviour>();

                if (_playersContainer.HasPlayer(playerBehaviour))
                {
                    _playersContainer.RemovePlayer(playerBehaviour);
                }
            }

            if (!runner.IsServer)
            {
                _buttonsController.ExitToMainMenu();
            }
        }
    }
}
