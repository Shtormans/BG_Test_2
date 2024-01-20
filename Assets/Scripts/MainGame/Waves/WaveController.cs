using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class WaveController : NetworkBehaviour
    {
        [SerializeField] private WaveContainer _waveContainer;
        [SerializeField] private SpawnersContainer _spawnersContainer;
        [SerializeField] private EnemyFabric _enemyFabric;
        [SerializeField] private BonusFabric _bonusFabric;

        private int _waveNumber = 0;
        private float _timeBetweenWaves = 1f;

        public event Action<int> NewWaveStarted;
        public event Action<int> EndOfWave;
        public event Action<float> TimeToNewWaveChanged;
        public event Action WavesAreOver;

        public void StartCreatingWaves()
        {
            StartCoroutine(AwaitForNextWave());
        }

        private void SpawnNewWave()
        {
            if (!_waveContainer.HasNextWave)
            {
                WavesAreOver?.Invoke();
                return;
            }

            _waveNumber++;
            var wave = _waveContainer.CurrentWave;
            RPC_SendNewWaveEvent(_waveNumber, wave.WaveTime);

            var enemies = _enemyFabric.CreateWave(wave);
            var bonuses = _bonusFabric.CreateWave(wave);

            SpawnEnemies(enemies);
            SpawnBonuses(bonuses);

            StartCoroutine(AwaitForEndOfCurrentWave());
        }

        private IEnumerator AwaitForNextWave()
        {
            _waveContainer.MoveToNextWave();
            RPC_SendEndOfWaveEvent(_waveNumber, _waveContainer.CurrentWave.RestTimeBeforeNextWave);

            float timePassed = 0f;
            float timeToNextWave = _waveContainer.CurrentWave.RestTimeBeforeNextWave;

            while (timePassed <= timeToNextWave)
            {
                yield return new WaitForSeconds(_timeBetweenWaves);

                timePassed += _timeBetweenWaves;
                RPC_SendTimeChangedEvent(timeToNextWave - timePassed);
            }

            SpawnNewWave();
        }

        private IEnumerator AwaitForEndOfCurrentWave()
        {
            float timePassed = 0f;
            float timeToNextWave = _waveContainer.CurrentWave.WaveTime;

            while (timePassed <= timeToNextWave)
            {
                yield return new WaitForSeconds(_timeBetweenWaves);

                timePassed += _timeBetweenWaves;
                RPC_SendTimeChangedEvent(timeToNextWave - timePassed);
            }

            StartCoroutine(AwaitForNextWave());
        }

        private void SpawnEnemies(List<EnemyController> enemies)
        {
            _spawnersContainer.ClearAfterSpawningWave();

            foreach (var enemy in enemies)
            {
                _spawnersContainer.SpawnEnemy(enemy);
            }
        }

        private void SpawnBonuses(List<BonusController> bonuses)
        {
            _spawnersContainer.ClearAfterSpawningWave();
         
            foreach (var bonus in bonuses)
            {
                _spawnersContainer.SpawnBonus(bonus);
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsHostPlayer)]
        private void RPC_SendNewWaveEvent(int waveNumber, float waveTime, RpcInfo info = default)
        {
            RPC_RelayNewWaveEvent(waveNumber, info.Source);
            RPC_RelayTimeChangedEvent(waveTime, info.Source);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsHostPlayer)]
        private void RPC_SendTimeChangedEvent(float secondsToNewWave, RpcInfo info = default)
        {
            RPC_RelayTimeChangedEvent(secondsToNewWave, info.Source);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsHostPlayer)]
        private void RPC_SendEndOfWaveEvent(int waveNumber, float secondsToNewWave, RpcInfo info = default)
        {
            RPC_RelayEndOfWaveEvent(waveNumber, info.Source);
            RPC_RelayTimeChangedEvent(_waveContainer.CurrentWave.RestTimeBeforeNextWave, info.Source);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
        private void RPC_RelayNewWaveEvent(int waveNumber, PlayerRef messageSource)
        {
            NewWaveStarted?.Invoke(waveNumber);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
        private void RPC_RelayTimeChangedEvent(float secondsToNewWave, PlayerRef messageSource)
        {
            TimeToNewWaveChanged?.Invoke(secondsToNewWave);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
        private void RPC_RelayEndOfWaveEvent(int currentWave, PlayerRef messageSource)
        {
            EndOfWave?.Invoke(currentWave);
        }
    }
}
