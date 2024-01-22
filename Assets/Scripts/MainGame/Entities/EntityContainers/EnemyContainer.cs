using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class EnemyContainer : MonoBehaviour
    {
        private List<EnemyController> _enemies;

        private void Awake()
        {
            _enemies = new List<EnemyController>();
        }

        public void AddEnemy(EnemyController enemy)
        {
            _enemies.Add(enemy);
        }

        public void DisabeBrains()
        {
            foreach (var enemy in _enemies)
            {
                enemy.enabled = false;
            }
        }
    }
}
