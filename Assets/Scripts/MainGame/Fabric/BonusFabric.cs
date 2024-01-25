using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class BonusFabric : MonoBehaviour
    {
        [SerializeField] private NetworkSpawner _networkSpawner;
        [SerializeField] private SpawnPosition _spawnPosition;

        public List<BonusController> CreateWave(Wave wave)
        {
            var bonuses = new List<BonusController>();

            foreach (var item in wave.Bonuses)
            {
                for (int i = 0; i < item.Amount; i++)
                {
                    var bonus = Spawn(item.Bonus);
                    bonuses.Add(bonus);
                }
            }

            return bonuses;
        }

        public BonusController CreateRandomBonusFromWave(Wave wave)
        {
            int index = Random.Range(0, wave.Enemies.Count);
            var bonus = wave.Bonuses[index].Bonus;

            return Spawn(bonus);
        }

        private BonusController Spawn(BonusController bonus)
        {
            return _networkSpawner.Runner.Spawn(bonus, _spawnPosition.transform.position, Quaternion.identity);
        }
    }
}
