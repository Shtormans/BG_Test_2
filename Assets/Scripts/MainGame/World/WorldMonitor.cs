using UnityEngine;

namespace MainGame
{
    public class WorldMonitor : MonoBehaviour
    {
        [SerializeField] private WaveController _waveController;
        [SerializeField] private NetworkSpawner _networkSpawner;

        public void Awake()
        {
            _networkSpawner.HostJoined += StartGame;
        }

        public void StartGame()
        {
            _waveController.SpawnNewWave();
        }
    }
}
