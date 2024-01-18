using Fusion;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class EnemyFabric : MonoBehaviour
    {
        [SerializeField] private PlayersContainer _playersContainer;
        [SerializeField] private NetworkSpawner _networkSpawner;

        public List<EnemyController> CreateWave(Wave wave)
        {
            var enemies = new List<EnemyController>();

            foreach (var item in wave.Enemies)
            {
                for (int i = 0; i < item.Amount; i++)
                {
                    var enemy = Spawn(item.Enemy);
                    enemy.SetPlayersContainer(_playersContainer);
                    
                    enemies.Add(enemy);
                }
            }

            return enemies;
        }

        private EnemyController Spawn(EnemyController enemy)
        {
            return _networkSpawner.Runner.Spawn(enemy, Vector3.zero, Quaternion.identity, _networkSpawner.CurrentPlayer);
        }
    }
}
