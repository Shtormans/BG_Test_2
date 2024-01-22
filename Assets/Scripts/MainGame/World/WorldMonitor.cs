using Cinemachine;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MainGame
{
    public class WorldMonitor : NetworkBehaviour
    {
        [SerializeField] private WaveController _waveController;
        [SerializeField] private PlayerFabric _playerFabric;
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private EnemyContainer _enemyContainer;

        [SerializeField] private List<GameObject> _awakeOnGameStartsObjects;
        [SerializeField] private Canvas _loadingCanvas;
        [SerializeField] private Canvas _gameUI;
        [SerializeField] private Canvas _gameEndScreen;

        [Inject]
        private PlayersContainer _playersContainer;

        public void Awake()
        {
            _playersContainer.PlayerSpawned += StartGame;
        }

        private void OnEnable()
        {
            _loadingCanvas.gameObject.SetActive(true);
        }

        public void OnPlayerDied()
        {
            if (_playersContainer.PlayersCount > 0)
            {
                _camera.Follow = _playersContainer.FirstPlayer.transform;
            }
            else
            {
                FinishGame();
            }
        }

        private void FinishGame()
        {
            if (HasStateAuthority)
            {
                _enemyContainer.DisabeBrains();
            }

            _gameUI.gameObject.SetActive(false);
            _gameEndScreen.gameObject.SetActive(true);
        }

        private void StartGame(PlayerBehaviour player)
        {
            _playersContainer.PlayerSpawned -= StartGame;
            _playersContainer.PlayerSpawned += BuildPlayer;

            AwakeGame();
            BuildPlayer(player);

            if (HasStateAuthority)
            {
                _waveController.StartCreatingWaves();
            }
        }

        private void BuildPlayer(PlayerBehaviour player)
        {
            StartCoroutine(AwaitToBuildPlayer(player));
        }

        private void AwakeGame()
        {
            _loadingCanvas.gameObject.SetActive(false);

            foreach (var item in _awakeOnGameStartsObjects)
            {
                item.SetActive(true);
            }
        }

        private IEnumerator AwaitToBuildPlayer(PlayerBehaviour player)
        {
            yield return new WaitUntil(() => _playerFabric.IsAllPlayerPartsSpawned(player));

            _playerFabric.UpdateSharedPlayer(player);

            if (player.HasInputAuthority)
            {
                _playerFabric.UpdateInputPlayer(player);
            }
        }
    }
}
