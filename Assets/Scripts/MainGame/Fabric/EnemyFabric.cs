using Fusion;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MainGame
{
    public class EnemyFabric : MonoBehaviour
    {
        [SerializeField] private NetworkSpawner _networkSpawner;
        [SerializeField] private EnemyContainer _enemyContainer;

        [Inject] private EnemyController.Factory _enemyFactory;

        public List<EnemyController> CreateWave(Wave wave)
        {
            var enemies = new List<EnemyController>();

            foreach (var item in wave.Enemies)
            {
                for (int i = 0; i < item.Amount; i++)
                {
                    var enemy = Spawn(item.Enemy);

                    enemies.Add(enemy);
                    _enemyContainer.AddEnemy(enemy);
                }
            }

            return enemies;
        }

        private EnemyController Spawn(EnemyController enemy)
        {
            return _enemyFactory.Create(enemy, _networkSpawner.Runner, _networkSpawner.CurrentPlayer);
        }
    }
}
