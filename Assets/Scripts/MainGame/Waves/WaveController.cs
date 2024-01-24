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
        [SerializeField] private float _timeToSpawnWave = 20f;

        private int _waveNumber = 0;
        private float _timeBetweenWaves = 1f;
        private Coroutine _spawnWaveDuringTimeCoroutine;

        public event Action<int> NewWaveStarted;
        public event Action<int> EndOfWave;
        public event Action<float> TimeToNewWaveChanged;
        public event Action WavesAreOver;

        public void StartCreatingWaves()
        {
            if (!HasStateAuthority) return;

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

            _spawnWaveDuringTimeCoroutine = StartCoroutine(SpawnWaveDuringTime());

            StartCoroutine(AwaitForEndOfCurrentWave());
        }

        private IEnumerator SpawnWaveDuringTime()
        {
            while (true)
            {
                yield return new WaitForSeconds(_timeToSpawnWave);

                var wave = _waveContainer.CurrentWave;

                var enemies = _enemyFabric.CreateWave(wave);
                var bonuses = _bonusFabric.CreateWave(wave);

                SpawnEnemies(enemies);
                SpawnBonuses(bonuses);
            }
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

            if (_spawnWaveDuringTimeCoroutine != null)
            {
                StopCoroutine(_spawnWaveDuringTimeCoroutine);
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

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_SendNewWaveEvent(int waveNumber, float waveTime, RpcInfo info = default)
        {
            NewWaveStarted?.Invoke(waveNumber);
            TimeToNewWaveChanged?.Invoke(waveTime);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_SendTimeChangedEvent(float secondsToNewWave, RpcInfo info = default)
        {
            TimeToNewWaveChanged?.Invoke(secondsToNewWave);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_SendEndOfWaveEvent(int waveNumber, float secondsToNewWave, RpcInfo info = default)
        {
            EndOfWave?.Invoke(waveNumber);
            TimeToNewWaveChanged?.Invoke(secondsToNewWave);
        }
    }
}
