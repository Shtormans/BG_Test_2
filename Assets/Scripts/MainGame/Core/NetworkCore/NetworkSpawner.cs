using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace MainGame
{
    public class NetworkSpawner : NetworkRunnerCallbacks
    {
        [SerializeField] private PlayerFabric _playerFabric;
        private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
        private NetworkRunner _runner;
        private PlayerRef _currentPlayer;

        public NetworkRunner Runner => _runner;
        public PlayerRef CurrentPlayer => _currentPlayer;

        private PlayersContainer _playersContainer;
        private MultisceneItemsTransfer _multisceneItemsTransfer;

        [Inject]
        private void Construct(PlayersContainer playersContainer, MultisceneItemsTransfer multisceneItemsTransfer)
        {
            _playersContainer = playersContainer;
            _multisceneItemsTransfer = multisceneItemsTransfer;
        }

        private void OnEnable()
        {
            _runner = _multisceneItemsTransfer.GetMultisceneItems().NetworkRunner;
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
        }
    }
}
