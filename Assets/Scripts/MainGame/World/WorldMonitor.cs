using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MainGame
{
    public class WorldMonitor : NetworkBehaviour
    {
        [SerializeField] private WaveController _waveController;
        [SerializeField] private PlayerFabric _playerFabric;

        [SerializeField] private List<GameObject> _awakeOnGameStartsObjects;
        [SerializeField] private Canvas _loadingCanvas;

        public void Awake()
        {
            PlayersContainer.PlayerSpawned += StartGame;
        }

        private void OnEnable()
        {
            _loadingCanvas.gameObject.SetActive(true);
        }

        private void StartGame(PlayerBehaviour player)
        {
            PlayersContainer.PlayerSpawned -= StartGame;
            PlayersContainer.PlayerSpawned += BuildPlayer;

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

            if (!player.HasStateAuthority)
            {
                _playerFabric.UpdateSharedPlayer(player);
            }

            if (player.HasInputAuthority)
            {
                _playerFabric.UpdateInputPlayer(player);
            }
        }
    }
}
