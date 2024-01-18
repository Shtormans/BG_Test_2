using Fusion;
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

        public void SpawnNewWave()
        {
            if (HasStateAuthority == false) return;

            var wave = _waveContainer.NextWave();

            var enemies = _enemyFabric.CreateWave(wave);
            var bonuses = _bonusFabric.CreateWave(wave);

            SpawnEnemies(enemies);
            SpawnBonuses(bonuses);
        }

        private void SpawnEnemies(List<EnemyController> enemies)
        {
            foreach (var enemy in enemies)
            {
                _spawnersContainer.SpawnEnemy(enemy);
            }
        }

        private void SpawnBonuses(List<BonusController> bonuses)
        {
            foreach (var bonus in bonuses)
            {
                _spawnersContainer.SpawnBonus(bonus);
            }
        }
    }
}
