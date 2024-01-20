using Fusion;
using UnityEngine;

namespace MainGame
{
    public class WorldMonitor : MonoBehaviour
    {
        [SerializeField] private WaveController _waveController;
        [SerializeField] private NetworkSpawner _networkSpawner;
        [Networked] private bool IsGameAlreadyExist { get; set; }

        public void Awake()
        {
            _networkSpawner.HostJoined += StartGame;
        }

        public void StartGame()
        {
            if (!IsGameAlreadyExist)
            {
                IsGameAlreadyExist = true;
                _waveController.StartCreatingWaves();
            }
        }
    }
}
