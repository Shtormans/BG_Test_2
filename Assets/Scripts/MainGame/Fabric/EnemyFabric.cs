using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class EnemyFabric
    {
        public List<EnemyController> CreateWave(Wave wave)
        {
            var enemies = new List<EnemyController>();

            foreach (var item in wave.Enemies)
            {
                for (int i = 0; i < item.Amount; i++)
                {
                    var enemy = Spawn(item.Enemy);
                    enemies.Add(enemy);
                }
            }

            return enemies;
        }

        private EnemyController Spawn(EnemyController enemy)
        {
            return GameObject.Instantiate(enemy, Vector3.zero, Quaternion.identity);
        }
    }
}
