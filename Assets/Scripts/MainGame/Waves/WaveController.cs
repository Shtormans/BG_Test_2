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
        [SerializeField] private float _timeToSpawnBonus = 15f;

        private readonly float _timeBetweenWaves = 1f;
        private int _waveNumber = 0;
        private bool _gameIsInRestState = false;
        private LinkedList<EnemyController> _aliveEnemies = new();
        private Coroutine _awaitForNextWaveCoroutine;
        private Coroutine _startSpawningBonusesCoroutine;

        public event Action<int> NewWaveStarted;
        public event Action<int> EndOfWave;
        public event Action<float> TimeToNewWaveChanged;
        public event Action WavesAreOver;

        public void StartCreatingWaves()
        {
            if (!HasStateAuthority) return;

            _startSpawningBonusesCoroutine = StartCoroutine(StartSpawningBonuses());
            _awaitForNextWaveCoroutine = StartCoroutine(AwaitForNextWave());
        }

        public void SkipRestBetweenWaves()
        {
            if (_awaitForNextWaveCoroutine != null)
            {
                StopCoroutine(_awaitForNextWaveCoroutine);
                SpawnNewWave();
            }
        }

        private void OnWaivesEnded()
        {
            if (_startSpawningBonusesCoroutine != null)
            {
                StopCoroutine(_startSpawningBonusesCoroutine);
            }

            WavesAreOver?.Invoke();
        }

        private void SpawnNewWave()
        {
            if (!_waveContainer.HasNextWave)
            {
                OnWaivesEnded();
                return;
            }

            _gameIsInRestState = false;
            _waveNumber++;
            var wave = _waveContainer.CurrentWave;
            RPC_SendNewWaveEvent(_waveNumber, wave.WaveTime);

            var enemies = _enemyFabric.CreateWave(wave);

            _aliveEnemies = new LinkedList<EnemyController>(enemies);

            AddKilledEventToEnemies(enemies);
            ArrangeEnemies(enemies);

            StartCoroutine(AwaitForEndOfCurrentWave());
        }

        private IEnumerator AwaitForNextWave()
        {
            _gameIsInRestState = true;
            _waveContainer.MoveToNextWave();
            RPC_SendEndOfWaveEvent(_waveNumber, _waveContainer.CurrentWave.RestTimeBeforeNextWave);
            KillAliveEnemies();

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

            _awaitForNextWaveCoroutine = StartCoroutine(AwaitForNextWave());
        }

        private void AddKilledEventToEnemies(List<EnemyController> enemies)
        {
            foreach (var enemy in enemies)
            {
                enemy.Died += OnEnemyWasKilled;
            }
        }

        private void OnEnemyWasKilled(Entity entity)
        {
            if (_gameIsInRestState)
            {
                return;
            }

            _aliveEnemies.Remove(entity as EnemyController);

            var enemy = _enemyFabric.CreateRandomEnemyFromWave(_waveContainer.CurrentWave);
            enemy.Died += OnEnemyWasKilled;
            _spawnersContainer.ArrangeEnemy(enemy);

            _aliveEnemies.AddLast(enemy);
        }

        private void OnBonusWasPickedUp(BonusController bonus)
        {
            _spawnersContainer.ClearBonusSpawner(bonus);
        }

        private IEnumerator StartSpawningBonuses()
        {
            while (true)
            {
                yield return new WaitForSeconds(_timeToSpawnBonus);

                if (_spawnersContainer.AmountOfOccupiedBonusSpawners == _spawnersContainer.AmountOfBonusSpawners)
                {
                    yield return null;
                }

                var bonus = _bonusFabric.CreateRandomBonusFromWave(_waveContainer.CurrentWave);
                bonus.PickedUp += OnBonusWasPickedUp;

                _spawnersContainer.ArrangeBonus(bonus);
            }
        }

        private void KillAliveEnemies()
        {
            foreach (var enemy in _aliveEnemies)
            {
                enemy.Kill();
            }

            _aliveEnemies.Clear();
        }

        private void ArrangeEnemies(List<EnemyController> enemies)
        {
            foreach (var enemy in enemies)
            {
                _spawnersContainer.ArrangeEnemy(enemy);
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
